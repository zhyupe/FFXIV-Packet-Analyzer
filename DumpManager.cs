using Advanced_Combat_Tracker;
using PacketAnalyzer.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketAnalyzer
{
    public static class DumpManager
    {
        static string dumpDir = Path.Combine();

        public static string Dump(Dictionary<string, string> param, byte[] buffer, int offset)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in param)
            {
                sb.Append(item.Key);
                sb.Append(": ");
                sb.Append(item.Value);
                sb.AppendLine();
            }

            if (offset >= 0)
            {
                string hexDump = Util.BuildHexDump(buffer, offset);
                sb.Append(hexDump);
            }

            return sb.ToString();
        }

        public static string Write(Dictionary<string, string> param, byte[] buffer, int offset)
        {
            string dumpPath = Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, "PacketDump", param["ID"] + ".txt");
            string dump = Dump(param, buffer, offset);
            File.WriteAllText(dumpPath, dump);
            return dump;
        }

    }
}
