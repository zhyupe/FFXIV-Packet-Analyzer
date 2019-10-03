using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace PacketAnalyzer.Network.IPC
{
    /**
     * Packet 00F7 (Public Message)
     * 0               4               8               12              16
     * +-------+-------+-------+-------+-------+-------+-------+-------+
     * | Unknown User Identifier       | Chara ID      | Serv  | C | ? |
     * +-------+-------+-------+-------+-------+-------+-------+-------+
     * | Sender Nick Name (32 bytes)                                   |
     * +-------+-------+-------+-------+-------+-------+-------+-------+
     * | Message Body (1008 bytes)                                     |
     * +-------+-------+-------+-------+-------+-------+-------+-------+
     */

    [StructLayout(LayoutKind.Explicit)]
    struct PublicMessageHeader
    {
        [FieldOffset(0)]
        public uint UID0;

        [FieldOffset(4)]
        public uint UID1;

        [FieldOffset(8)]
        public uint CharacterID;

        [FieldOffset(12)]
        public ushort UserServer;

        [FieldOffset(14)]
        public PublicMessageType Type;

        [FieldOffset(15)]
        public byte Reserved0;
    }

    enum PublicMessageType : byte {
        Shout = 0x0B,
        Yell = 0x1E,
        Say = 0x0A
    }

    class PublicMessage : IPCBase
    {
        public PublicMessageHeader Header;
        public string Nick;
        public string Content;

        public PublicMessage(byte[] message, int offset)
        {
            int headerLength = PacketParser.ParsePacket(message, offset, out Header);

            Nick = Encoding.UTF8.GetString(message, offset + headerLength, 32).TrimEnd((char)0);
            Content = Encoding.UTF8.GetString(message, offset + headerLength + 32, message.Length - (offset + headerLength + 32)).TrimEnd((char)0);
        }

        public PublicMessage WriteParams(Dictionary<string, string> parsedValues)
        {
            parsedValues.Add("Message-Reserved", string.Format("{0:X2}", Header.Reserved0));
            parsedValues.Add("Message-Type", Header.Type.ToString());
            parsedValues.Add("User-Id", string.Format("{0:X8}", Header.CharacterID));
            parsedValues.Add("User-Id-Unknown", string.Format("{0:X8} {1:X8}", Header.UID0, Header.UID1));
            parsedValues.Add("User-Server", Header.UserServer.ToString());

            parsedValues.Add("User-Nick", Nick);
            parsedValues.Add("Content", Content);

            return this;
        }
    }
}
