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
            if (openFileDialog1.FileName == "") return;

            string content = File.ReadAllText(openFileDialog1.FileName);
            Dictionary<string, string> parsedValues = Parse(content, out var dump);

            Dictionary<string, string> newValues = new Dictionary<string, string>();
              var ipc = PacketParser.ParseIPCPacket((ServerZoneIpcType)short.Parse(parsedValues["IPC-Type"], System.Globalization.NumberStyles.HexNumber), dump, 0, newValues);


            if (dump == null)
            {
                DumpBox.Text = "(no hex dump found)";
            } else
            {
                DumpBox.Text = DumpManager.Dump(newValues, dump, 0);
            }

        }
    }
}
