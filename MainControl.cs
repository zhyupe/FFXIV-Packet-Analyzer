using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Advanced_Combat_Tracker;
using System.IO;
using PacketAnalyzer.Network;

namespace PacketAnalyzer
{
    public partial class MainControl : UserControl, IActPluginV1
    {
        public MainControl()
        {
            InitializeComponent();
            random = new Random();
            PacketList.DoubleBuffering(true);
        }

        Label pluginStatusText = null;
        private ParsePlugin parsePlugin = null;
        Random random;

        public void DeInitPlugin()
        {
            if (this.pluginStatusText != null)
            {
                this.pluginStatusText.Text = "Packet Analyzer Unloaded.";
                this.pluginStatusText = null;
            }

            parsePlugin.Stop();
        }

        public void InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText)
        {
            foreach (ActPluginData plugin in ActGlobals.oFormActMain.ActPlugins)
            {
                if (plugin.pluginObj != this) continue;
                DB.Root = plugin.pluginFile.Directory.FullName;
                break;
            }

            try
            {
                this.pluginStatusText = pluginStatusText;
                this.pluginStatusText.Text = "Packet Analyzer Started.";
                pluginScreenSpace.Text = "Packet Analyzer";
                pluginScreenSpace.Controls.Add(this);
                this.Dock = DockStyle.Fill;
                this.Scale(new SizeF(2, 2));

                if (parsePlugin == null)
                {
                    IActPluginV1 ffxiv_plugin = null;
                    foreach (var plugin in ActGlobals.oFormActMain.ActPlugins)
                    {
                        if (plugin.pluginFile.Name == "FFXIV_ACT_Plugin.dll")
                        {
                            ffxiv_plugin = plugin.pluginObj;
                            break;
                        }
                    }

                    if (ffxiv_plugin == null)
                    {
                        log("This plugin requires FFXIV_ACT_Plugin to work.");
                    }
                    else
                    {
                        parsePlugin = new ParsePlugin(ffxiv_plugin);
                        var Network = new FFXIVNetworkMonitor();
                        Network.onException += logException;
                        Network.onReceiveEvent += Network_onReceiveEvent;
                        Network.onSendEvent += Network_onSendEvent;
                        parsePlugin.Network = Network;
                        parsePlugin.Start();
                    }
                }
            }
            catch (Exception e)
            {
                logException(e);
            }

            initListHeader();
        }

        private void Network_onSendEvent(string connection, long epoch, byte[] message)
        {
            parsePacket(message, true);
        }

        private void Network_onReceiveEvent(string connection, long epoch, byte[] message)
        {
            parsePacket(message, false);
        }

        private void initListHeader()
        {
            PacketList.Columns.Clear();
            PacketList.Columns.Add("ID", 200);
            PacketList.Columns.Add("Source", 100);
            PacketList.Columns.Add("Target", 100);
            PacketList.Columns.Add("SType");
            PacketList.Columns.Add("IPC-Type");
            PacketList.Columns.Add("Data");
        }

        private void parsePacket(byte[] message, bool isClient = false)
        {
            if (!EnabledBox.Checked) return;

            var pktHeaderLength = PacketParser.ParsePacket<FFXIVSegmentHeader>(message, 0, out var pktHeader);
            var parsedValues = new Dictionary<string, string>();
            parsedValues.Add("ID", string.Format("{1}{0}-{2:X4}", isClient ? 'C' : 'S', DateTime.UtcNow.EpochMillis(), random.Next(65535)));
            parsedValues.Add("SSize", string.Format("{0}", pktHeader.Size));
            parsedValues.Add("Source", string.Format("{0:X8}", pktHeader.SourceActorId));
            parsedValues.Add("Target", string.Format("{0:X8}", pktHeader.TargetActorId));
            parsedValues.Add("SType", pktHeader.SegmentType.ToString());
            parsedValues.Add("SReserved", string.Format("{0:X4}", pktHeader.Reserved));

            int dumpOffset = pktHeaderLength;
            switch (pktHeader.SegmentType) {
                case SegmentType.ClientKeepAlive:
                case SegmentType.ServerKeepAlive:
                    PacketParser.ParsePacket<FFXIVKeepAliveData>(message, pktHeaderLength, out var pktBody);
                    parsedValues.Add("Data", string.Format("Id={0}, Timestamp={1}", pktBody.Id, pktBody.Timestamp));
                    AddToPacketList(parsedValues);
                    break;
                case SegmentType.IPC:
                    var ipcHeaderLength = PacketParser.ParsePacket<FFXIVIpcHeader>(message, pktHeaderLength, out var ipcHeader);
                    dumpOffset = pktHeaderLength + ipcHeaderLength;
                    parsedValues.Add("IPC-Reserved", string.Format("0:{0:X2} 1:{1:X2} 4: {2:X4} 12: {3:X8}", ipcHeader.Reserved1, ipcHeader.Reserved2, ipcHeader.Unknown2, ipcHeader.UnknownC));

                    Type EnumType = isClient ? typeof(ClientZoneIpcType) : typeof(ServerZoneIpcType);
                    parsedValues.Add("IPC-Type", string.Format("{0:X4}", ipcHeader.Type));
                    IPCBase ipc = null;
                    if (Enum.IsDefined(EnumType, ipcHeader.Type))
                    {
                        parsedValues.Add("IPC-TypeName", Enum.GetName(EnumType, ipcHeader.Type));
                        ipc = PacketParser.ParseIPCPacket((ServerZoneIpcType)ipcHeader.Type, message, dumpOffset, parsedValues);
                    }
                    else
                    {
                        parsedValues.Add("Data", string.Format("Server={0}, Timestamp={1}", ipcHeader.ServerId, ipcHeader.Timestamp));
                    }

                    string dump = DumpManager.Dump(parsedValues, message, dumpOffset);
                    if (ipc == null || ipc.Dump)
                    {
                        DumpManager.Write(parsedValues, dump);
                    }

                    if (ipc == null || ipc.Display)
                    {
                        AddToPacketList(parsedValues, dump);
                    }
                    break;
                default:
                    DumpManager.Write(parsedValues, message, dumpOffset);
                    break;
            }
        }

        void AddToPacketList(Dictionary<string, string> parsedValues, string dump = "(empty)")
        {
            ListViewItem item = new ListViewItem();
            item.Text = parsedValues["ID"];

            for (int i = 1; i < PacketList.Columns.Count; ++i)
            {
                var key = PacketList.Columns[i].Text;
                item.SubItems.Add(parsedValues.ContainsKey(key) ? parsedValues[key] : "");
            }

            item.Tag = dump;
            PacketList.Items.Add(item);
        }

        void logException(Exception e)
        {
            log(string.Format("[E][{0}]{1}\r\n{2}", e.GetType(), e.Message, e.StackTrace));
        }

        void log(string message)
        {
            MessageBox.Show(message, "FFXIV - Packet Analyzer");
        }

        private void PacketList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PacketList.SelectedItems.Count == 0)
            {
                DumpBox.Text = "";
            } else
            {
                DumpBox.Text = (string)PacketList.SelectedItems[0].Tag;
            }
            
        }
    }
}
