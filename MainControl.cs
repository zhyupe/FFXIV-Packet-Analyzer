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
using System.Threading;

namespace PacketAnalyzer
{
    public partial class MainControl : UserControl, IActPluginV1
    {
        public MainControl()
        {
            InitializeComponent();
            random = new Random();
            PacketList.DoubleBuffering(true);
            initListHeader();
        }

        Label pluginStatusText = null;
        private ParsePlugin parsePlugin = null;
        private Thread networkThread = null;
        private bool _characterInited = false;
        private bool characterInited {
            get { return _characterInited; }
            set
            {
                _characterInited = value;
                characterButton.Text = value ? "View Character" : "Load Character";
            }
        }

        private volatile Dictionary<ushort, bool> IpcTypeFilter = new Dictionary<ushort, bool>();
        Random random;

        public void DeInitPlugin()
        {
            if (this.pluginStatusText != null)
            {
                this.pluginStatusText.Text = "Packet Analyzer Unloaded.";
                this.pluginStatusText = null;
            }

            parsePlugin.Stop();

            if (networkThread != null)
            {
                networkThread.Abort();
                networkThread = null;
            }
        }

        public void InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText)
        {
            if (!characterInited)
            {
                string path = Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, "PacketDump", "Character.dat");
                Character.Init(path);
                characterInited = true;
            }

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

                if (parsePlugin == null)
                {
                    IActPluginV1 ffxivPlugin = null;
                    foreach (var plugin in ActGlobals.oFormActMain.ActPlugins)
                    {
                        if (plugin.pluginFile.Name == "FFXIV_ACT_Plugin.dll")
                        {
                            ffxivPlugin = plugin.pluginObj;
                            break;
                        }
                    }

                    if (ffxivPlugin == null)
                    {
                        log("This plugin requires FFXIV_ACT_Plugin to work.");
                    }
                    else
                    {
                        networkThread = new Thread(() => NetworkWorker(ffxivPlugin));
                        networkThread.Start();
                    }
                }
            }
            catch (Exception e)
            {
                logException(e);
            }

        }

        private void NetworkWorker (IActPluginV1 ffxivPlugin)
        {
            parsePlugin = new ParsePlugin(ffxivPlugin);
            var Network = new FFXIVNetworkMonitor();
            Network.onException += logException;
            Network.onReceiveEvent += (string connection, long epoch, byte[] message) =>
            {
                parsePacket(message, false);
            };
            Network.onSendEvent += (string connection, long epoch, byte[] message) =>
            {
                parsePacket(message, true);
            }; ;
            parsePlugin.Network = Network;
            parsePlugin.Start();
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
            parsePacket(message, DateTime.Now, isClient, false);
        }

        private void parsePacket(byte[] message, DateTime time, bool isClient = false, bool isReplay = false)
        {
            if (!EnabledBox.Checked) return;
            if (!isReplay)
            {
                DumpManager.Pcap(message);
            }

            var pktHeaderLength = PacketParser.ParsePacket<FFXIVSegmentHeader>(message, 0, out var pktHeader);
            var parsedValues = new Dictionary<string, string>();
            parsedValues.Add("ID", string.Format("{1}{0}", isClient ? 'C' : 'S', time.ToString("yyyyMMdd-HHmmss-ffffff")));
            parsedValues.Add("SSize", string.Format("{0}", pktHeader.Size));
            parsedValues.Add("Source", string.Format("{0:X8}", pktHeader.SourceActorId));
            parsedValues.Add("Target", string.Format("{0:X8}", pktHeader.TargetActorId));
            parsedValues.Add("SType", pktHeader.SegmentType.ToString());
            parsedValues.Add("SReserved", string.Format("{0:X4}", pktHeader.Reserved));

            int dumpOffset = pktHeaderLength;
            switch (pktHeader.SegmentType) {
                case SegmentType.ClientKeepAlive:
                case SegmentType.ServerKeepAlive:
                    // PacketParser.ParsePacket<FFXIVKeepAliveData>(message, pktHeaderLength, out var pktBody);
                    // parsedValues.Add("Data", string.Format("Id={0}, Timestamp={1}", pktBody.Id, pktBody.Timestamp));
                    // AddToPacketList(parsedValues);
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
                        // DumpManager.Write(parsedValues, dump);
                    }

                    if (ipc == null || ipc.Display)
                    {
                        if (!IpcTypeFilter.ContainsKey(ipcHeader.Type))
                        {
                            IpcTypeFilter.Add(ipcHeader.Type, true);
                            var item = new ListViewItem();
                            item.Tag = ipcHeader.Type;
                            if (parsedValues.ContainsKey("IPC-TypeName"))
                            {
                                item.Text = string.Format("{0} - {1}", parsedValues["IPC-Type"], parsedValues["IPC-TypeName"]);
                            } else
                            {
                                item.Text = parsedValues["IPC-Type"];
                            }

                            item.Checked = true;
                            ipcTypesList.Invoke((MethodInvoker)(() => ipcTypesList.Items.Add(item)));
                        }

                        if (IpcTypeFilter[ipcHeader.Type])
                        {
                            AddToPacketList(new PacketItemDetail()
                            {
                                ipc = ipc,
                                ipcHeader = ipcHeader,
                                segmentHeader = pktHeader,
                                dump = dump,
                                parsedValues = parsedValues
                            });
                        }
                    }
                    break;
                default:
                    if (!isReplay)
                    {
                        DumpManager.Write(parsedValues, message, dumpOffset);
                    }
                    break;
            }
        }

        void AddToPacketList(PacketItemDetail detail)
        {
            ListViewItem item = new ListViewItem();
            item.Text = detail.parsedValues["ID"];

            for (int i = 1; i < PacketList.Columns.Count; ++i)
            {
                var key = PacketList.Columns[i].Text;
                item.SubItems.Add(detail.parsedValues.ContainsKey(key) ? detail.parsedValues[key] : "");
            }

            item.Tag = detail;
            if (detail.ipc != null)
            {
                item.BackColor = Color.AliceBlue;
            }
            PacketList.Invoke((MethodInvoker)(() => PacketList.Items.Add(item)));
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
                DumpBox.Text = ((PacketItemDetail)PacketList.SelectedItems[0].Tag).dump;
            }
            
        }

        private async void Button1_Click(object sender, EventArgs e)
        {
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Pcap Dump|*.pcap";
            if (openFileDialog.ShowDialog() != DialogResult.OK) return;

            PacketList.BeginUpdate();
            await DumpManager.LoadPcap(openFileDialog.FileName, (byte[] message, DateTime time) =>
            {
                parsePacket(message, time, false, true);
            }, new Progress<float>(percent => DumpBox.Text = string.Format("Loading: {0:0.00}%", percent)));
            PacketList.EndUpdate();
        }

        private void EnabledBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!EnabledBox.Checked)
            {
                DumpManager.FlushPcap();
            }
        }

        private void FilterButton_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in ipcTypesList.Items)
            {
                IpcTypeFilter[(ushort)item.Tag] = item.Checked;
            }

            foreach (ListViewItem item in ipcTypesList.Items)
            {
                PacketItemDetail detail = (PacketItemDetail)item.Tag;
                if (!IpcTypeFilter[detail.ipcHeader.Type])
                {
                    item.Remove();
                }
            }
        }

        struct PacketItemDetail
        {
            public FFXIVSegmentHeader segmentHeader;
            public FFXIVIpcHeader ipcHeader;
            public IPCBase ipc;
            public string dump;
            public Dictionary<string, string> parsedValues;
        }

        private void CharacterButton_Click(object sender, EventArgs e)
        {
            if (characterInited)
            {
                DumpBox.Text = Character.Dump();
            }
            else
            {
                openFileDialog.Multiselect = false;
                openFileDialog.Filter = "Character Database|Character.dat";
                if (openFileDialog.ShowDialog() != DialogResult.OK) return;
                Character.Init(openFileDialog.FileName);

                characterInited = true;
            }
        }
    }
}
