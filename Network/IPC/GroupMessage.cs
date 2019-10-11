using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace PacketAnalyzer.Network.IPC
{
    /**
     * Packet 0065 (Group Message)
     * 0               4               8               12              16
     * +-------+-------+-------+-------+-------+-------+-------+-------+
     * | Group ID      | Type  | Serv  | Unknown User Identifier       |
     * +-------+-------+-------+-------+-------+-------+-------+-------+
     * | Chara ID      | Serv  | 0 | Sender Nick Name (32 bytes)       |
     * +-------+-------+-------+-------+-------+-------+-------+-------+
     * | (continue)                                                    |
     * +-------+-------+-------+-------+-------+-------+-------+-------+
     * | (continue)                | Message (1023 bytes)              |
     * +-------+-------+-------+-------+-------+-------+-------+-------+
     */

    [StructLayout(LayoutKind.Explicit)]
    struct GroupMessageHeader
    {
        [FieldOffset(0)]
        public uint GroupID;

        [FieldOffset(4)]
        public GroupMessageType Type;

        [FieldOffset(6)]
        public FFXIVChinaServer Server;

        [FieldOffset(8)]
        public uint UID0;

        [FieldOffset(12)]
        public uint UID1;

        [FieldOffset(16)]
        public uint CharacterID;

        [FieldOffset(20)]
        public FFXIVChinaServer UserServer;

        [FieldOffset(22)]
        public byte Reserved0;
    }

    enum GroupMessageType: ushort {
        Linkshell = 2,
        FreeCompany = 3,
        NoviceNetwork = 4,

        // Not sure
        Time = 0x014d
    }

    class GroupMessage : IPCBase
    {
        public GroupMessageHeader Header;
        public string Nick;
        public string Content;

        public GroupMessage(byte[] message, int offset)
        {
            int headerLength = 23;
            PacketParser.ParsePacket(message, offset, out Header);

            switch (Header.Type)
            {
                case GroupMessageType.FreeCompany:
                case GroupMessageType.Linkshell:
                    Nick = Encoding.UTF8.GetString(message, offset + headerLength, 32).TrimEnd((char)0);
                    Content = Encoding.UTF8.GetString(message, offset + headerLength + 32, message.Length - (offset + headerLength + 32)).TrimEnd((char)0);

                    Character.Ensure(Header.UserServer, Header.CharacterID, Nick);
                    break;
                case GroupMessageType.Time:
                    break;
                default:
                    Dump = true;
                    break;
            }
        }

        public GroupMessage WriteParams(Dictionary<string, string> parsedValues)
        {
            if (Header.Type == GroupMessageType.Time)
            {
                parsedValues.Add("Data", string.Format("<ServerTime> {0}", Header.GroupID));
                return this;
            }

            parsedValues.Add("Message-Reserved", string.Format("{0:X2}", Header.Reserved0));
            parsedValues.Add("Group-Id", string.Format("{1:X8} ({0})", Header.Type.ToString(), Header.GroupID));
            parsedValues.Add("Group-Server", Header.Server.ToString());
            parsedValues.Add("User-Id", string.Format("{0:X8} {1:X8} {2:X8}", Header.UID0, Header.UID1, Header.CharacterID));
            parsedValues.Add("User-Server", Header.UserServer.ToString());

            switch (Header.Type)
            {
                case GroupMessageType.FreeCompany:
                case GroupMessageType.Linkshell:
                    parsedValues.Add("User-Nick", Nick);
                    parsedValues.Add("Content", Content);

                    parsedValues.Add("Data", string.Format("<{0}> {1}", Nick, Content));
                    break;
                default:
                    parsedValues.Add("Data", string.Format("Type={0}", Header.Type));
                    break;
            }

            return this;
        }
    }
}
