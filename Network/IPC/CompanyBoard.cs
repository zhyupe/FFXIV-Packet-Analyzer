using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace PacketAnalyzer.Network.IPC
{
    /**
     * Packet 013F (Company Board)
     * 0               4               8               12              16
     * +-------+-------+-------+-------+-------+-------+-------+-------+
     * | 1 | Message ...                                               |
     * +-------+-------+-------+-------+-------+-------+-------+-------+
     */

    class CompanyBoard : IPCBase
    {
        public byte Unknown;
        public string Content;

        public CompanyBoard(byte[] message, int offset)
        {
            Unknown = message[offset];
            Content = Encoding.UTF8.GetString(message, offset + 1, message.Length - (offset + 1)).TrimEnd((char)0);
        }

        public CompanyBoard WriteParams(Dictionary<string, string> parsedValues)
        {
            parsedValues.Add("Unknown", string.Format("{0:X2}", Unknown));
            parsedValues.Add("Content", Content);
            parsedValues.Add("Data", string.Format("[Company Board] {0}", Content));

            return this;
        }
    }
}
