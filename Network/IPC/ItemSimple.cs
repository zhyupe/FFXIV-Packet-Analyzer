using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace PacketAnalyzer.Network.IPC
{
    /**
     * Packet 019B (ItemSimple)
     * 0               4               8               12              16
     * +-------+-------+-------+-------+-------+-------+-------+-------+
     * | Index         | Loc   | Pos   | Amount        | Unknown       |
     * +-------+-------+-------+-------+-------+-------+-------+-------+
     * | ItemID| (0)                                                   |
     * +-------+-------+-------+-------+-------+-------+-------+-------+
     */

    [StructLayout(LayoutKind.Explicit)]
    struct ItemSimpleBody
    {
        [FieldOffset(0)]
        public uint Index;

        [FieldOffset(4)]
        public ItemLocation Location;

        [FieldOffset(5)]
        public ushort Position;

        [FieldOffset(8)]
        public uint Amount;

        [FieldOffset(12)]
        public uint Unknown0;

        [FieldOffset(16)]
        public ushort ItemID;

        [FieldOffset(18)]
        public ushort Unknown1;

        [FieldOffset(20)]
        public uint Unknown2;

        [FieldOffset(24)]
        public uint Unknown3;

        [FieldOffset(28)]
        public uint Unknown4;
    }

    class ItemSimple : IPCBase
    {
        public ItemSimpleBody Body;

        public ItemSimple(byte[] message, int offset)
        {
            PacketParser.ParsePacket(message, offset, out Body);
        }

        public ItemSimple WriteParams(Dictionary<string, string> parsedValues)
        {
            string name = DB.Get("Item").FindById(Body.ItemID)["Name"];

            parsedValues.Add("Index", Body.Index.ToString());
            parsedValues.Add("Item-ID", Body.ItemID.ToString());
            parsedValues.Add("Item-Name", name);
            parsedValues.Add("Item-Pos", string.Format("{0} - {1}", Body.Location, Body.Position));

            parsedValues.Add("Item-Amount", Body.Amount.ToString());
            parsedValues.Add("Unknown", string.Format("{0:X4} {1:X2} {2:X4} {3:X4} {4:X4}",
                Body.Unknown0, Body.Unknown1, Body.Unknown2, Body.Unknown3, Body.Unknown4));

            parsedValues.Add("Data", string.Format("<{0}> {1}: {2}", Body.Location, name, Body.Amount));
            return this;
        }
    }
}
