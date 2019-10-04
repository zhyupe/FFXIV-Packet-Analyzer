using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace PacketAnalyzer.Network.IPC
{
    /**
     * 0               4               8               12              16
     * +-------+-------+-------+-------+-------+-------+-------+-------+
     * | Type  | ?     | Fate ID       | Data1         | Data 2        |
     * +-------+-------+-------+-------+-------+-------+-------+-------+
     * | (?)                                           | Data 6        |
     * +-------+-------+-------+-------+-------+-------+-------+-------+
    */
    [StructLayout(LayoutKind.Explicit)]
    struct ActorControl143Body
    {
        [FieldOffset(0)]
        public ActorControl143Type Type;

        [FieldOffset(2)]
        public ushort Unknown0;

        [FieldOffset(4)]
        public uint Data0;

        [FieldOffset(8)]
        public uint Data1;

        [FieldOffset(12)]
        public uint Data2;

        [FieldOffset(16)]
        public uint Data3;

        [FieldOffset(20)]
        public uint Data4;

        [FieldOffset(24)]
        public uint Data5;

        [FieldOffset(28)]
        public uint Data6;
    }

    enum ActorControl143Type : ushort
    {
        FateStart = 0x74,
        // FateStart = 0x78, (?)
        FateEnd = 0x79,
        /**
         * Data1: Progress (0-100)
         */
        FateProgress = 0x9B,
        /**
         * Data1: BNpcBase
         * Data2: BNpcName
         * Data6: BNpcYell (?)
         */
        FateNpc = 0xB2,
        /**
         * Data0: Index
         * Data1: ItemID
         * Data2: Amount
         */
        PlayerCurrency = 0x017a,
    }
    class ActorControl143 : IPCBase
    {
        public ActorControl143Body Body;

        public ActorControl143(byte[] message, int offset)
        {
            PacketParser.ParsePacket(message, offset, out Body);
        }

        public ActorControl143 WriteParams(Dictionary<string, string> parsedValues)
        {
            parsedValues.Add("143-Type", Body.Type.ToString());
            parsedValues.Add("143-Unknown", Body.Unknown0.ToString("X4"));
            parsedValues.Add("143-Data", string.Format("0:{0:X8} 1:{1:X8} 2:{2:X8} 3:{3:X8} 4:{4:X8} 5:{5:X8}", Body.Data1, Body.Data2, Body.Data3, Body.Data4, Body.Data5, Body.Data6));

            switch (Body.Type)
            {
                case ActorControl143Type.FateStart:
                case ActorControl143Type.FateEnd:
                    string fateName1 = DB.Get("Fate").FindById(Body.Data0)["Name"];
                    parsedValues.Add("Fate-ID", Body.Data0.ToString());
                    parsedValues.Add("Fate-Name", fateName1);
                    parsedValues.Add("Data", string.Format("<{0}> {1}", Body.Type, fateName1));
                    break;
                case ActorControl143Type.FateProgress:
                    string fateName2 = DB.Get("Fate").FindById(Body.Data0)["Name"];
                    parsedValues.Add("Fate-ID", Body.Data0.ToString());
                    parsedValues.Add("Fate-Name", fateName2);
                    parsedValues.Add("Fate-Progress", Body.Data1.ToString());
                    parsedValues.Add("Data", string.Format("<{0}> {1}: {2}%", Body.Type, fateName2, Body.Data1));
                    break;
                case ActorControl143Type.FateNpc:
                    string fateName3 = DB.Get("Fate").FindById(Body.Data0)["Name"];
                    parsedValues.Add("Fate-ID", Body.Data0.ToString());
                    parsedValues.Add("Fate-Name", fateName3);
                    parsedValues.Add("Npc-Name", Body.Data1.ToString());
                    parsedValues.Add("Npc-ID", Body.Data2.ToString());
                    parsedValues.Add("Npc-Yell", Body.Data6.ToString());

                    parsedValues.Add("Data", string.Format("<{0}> {1}: {2}", Body.Type, fateName3, Body.Data2));
                    break;
                case ActorControl143Type.PlayerCurrency:
                    string name = DB.Get("Item").FindById(Body.Data1)["Name"];
                    parsedValues.Add("Currency-ID", Body.Data1.ToString());
                    parsedValues.Add("Currency-Name", name);
                    parsedValues.Add("Currency-Amount", Body.Data2.ToString());

                    parsedValues.Add("Data", string.Format("<{0}> {1}: {2}", Body.Type, name, Body.Data2));
                    break;
                default:
                    parsedValues.Add("Data", string.Format("<{0}>", Body.Type));
                    break;
            }

            return this;
        }
    }
}
