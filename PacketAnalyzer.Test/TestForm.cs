using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PacketAnalyzer.Network;

namespace PacketAnalyzer.Test
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
            initListHeader();
        }

        public Dictionary<string, string> Parse(string content, out byte[] dump)
        {
            dump = null;

            Dictionary<string, string> param = new Dictionary<string, string>();
            int pos = 0;
            while (pos < content.Length && content[pos] != '#')
            {
                int valuePos = content.IndexOf(": ", pos);
                int linePos = content.IndexOf('\n', pos);

                param.Add(content.Substring(pos, valuePos - pos).Trim(), content.Substring(valuePos + 2, linePos - valuePos - 2).Trim());
                pos = linePos + 1;
            }

            if (pos >= content.Length) return param;

            if (content[pos] == '#' && content.Substring(pos, 8) == "#ADDRESS")
            {
                var lines = content.Substring(pos).Split('\n').Skip(2);
                dump = new byte[lines.Count() * 16];
                int dumpPos = -1;

                foreach (var line in lines)
                {
                    if (string.IsNullOrEmpty(line)) continue;
                    foreach (var col in line.Split(':')[1].Split(' '))
                    {
                        if (string.IsNullOrEmpty(col)) continue;

                        dump[++dumpPos] = byte.Parse(col, System.Globalization.NumberStyles.HexNumber);
                    }
                }

                Array.Resize(ref dump, ++dumpPos);
            }

            return param;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();

            foreach (var fileName in openFileDialog1.FileNames)
            {
                if (fileName == "") return;

                string content = File.ReadAllText(fileName);
                Dictionary<string, string> parsedValues = Parse(content, out var dump);

                Dictionary<string, string> newValues = new Dictionary<string, string>();

                if (parsedValues.ContainsKey("IPC-Type"))
                {
                    var ipc = PacketParser.ParseIPCPacket((ServerZoneIpcType)short.Parse(parsedValues["IPC-Type"], System.Globalization.NumberStyles.HexNumber), dump, 0, newValues);

                    for (int i = 0; i < PacketList.Columns.Count; ++i)
                    {
                        var key = PacketList.Columns[i].Text;
                        if (parsedValues.ContainsKey(key) && !newValues.ContainsKey(key))
                        {
                            newValues.Add(key, parsedValues[key]);
                        }
                    }

                    AddToPacketList(newValues, DumpManager.Dump(newValues, dump, dump == null ? -1 : 0));
                }
                else
                {
                    AddToPacketList(parsedValues, DumpManager.Dump(parsedValues, dump, dump == null ? -1 : 0));
                }
            }
        }

        private void initListHeader()
        {
            PacketList.Columns.Clear();
            PacketList.Columns.Add("ID", 190);
            PacketList.Columns.Add("Source", 80);
            PacketList.Columns.Add("Target", 80);
            PacketList.Columns.Add("IPC-Type", 80);
            PacketList.Columns.Add("Data", 400);
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
        private void PacketList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PacketList.SelectedItems.Count == 0)
            {
                DumpBox.Text = "";
            }
            else
            {
                DumpBox.Text = (string)PacketList.SelectedItems[0].Tag;
            }

        }
    }
}
