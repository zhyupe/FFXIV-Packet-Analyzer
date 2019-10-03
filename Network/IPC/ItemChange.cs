using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace PacketAnalyzer.Network.IPC
{
    /**
     * Packet 019E (ItemChange)
     * 0               4               8               12              16
     * +-------+-------+-------+-------+-------+-------+-------+-------+
     * | Index         | Unknown       | Loc   | Pos   | Amount        |
     * +-------+-------+-------+-------+-------+-------+-------+-------+
     * | ItemID| (0)                                                   |
     * +-------+-------+-------+-------+-------+-------+-------+-------+
     * |HQ?| ? | Condi?| Spiri?|       | Shadow|                       |
     * +-------+-------+-------+-------+-------+-------+-------+-------+
     * | (?)                                                           |
     * +-------+-------+-------+-------+-------+-------+-------+-------+
     */

    [StructLayout(LayoutKind.Explicit)]
    struct ItemChangeBody
    {
        [FieldOffset(0)]
        public uint Index;

        [FieldOffset(4)]
        public uint Unknown0;

        [FieldOffset(8)]
        public ItemChangeLocation Location;

        [FieldOffset(10)]
        public ushort Position;

        [FieldOffset(12)]
        public uint Amount;

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

        [FieldOffset(32)]
        public byte Attribute1;

        [FieldOffset(33)]
        public byte Attribute2;

        [FieldOffset(34)]
        public ushort Condition;

        [FieldOffset(36)]
        public ushort Spiritbond;

        [FieldOffset(38)]
        public ushort Unknown5;

        [FieldOffset(40)]
        public ushort GlamourID;

        [FieldOffset(42)]
        public uint Unknown6;

        [FieldOffset(44)]
        public uint Unknown7;

        [FieldOffset(48)]
        public uint Unknown8;

        [FieldOffset(52)]
        public uint Unknown9;

        [FieldOffset(56)]
        public uint Unknown10;

        [FieldOffset(60)]
        public uint Unknown11;
    }

    enum ItemChangeLocation : ushort
    {
        Armoury = 0x03e8,
        Currency = 0x07d0,
        SoulCrystal = 0x0d48,

        Inventory1 = 0x0001,
    }
    enum ItemChangeArmouryPosition : ushort {
        MainHand,
        OffHand,
        Head,
        Body,
        Hands,
        Waist,
        Legs,
        Feet,
        Ears,
        Neck,
        Wrists,
        RightRing,
        LeftRing
    }

    class ItemChange : IPCBase
    {
        public ItemChangeBody Body;

        public ItemChange(byte[] message, int offset)
        {
            PacketParser.ParsePacket(message, offset, out Body);
        }

        public ItemChange WriteParams(Dictionary<string, string> parsedValues)
        {
            parsedValues.Add("Index", Body.Index.ToString());
            parsedValues.Add("Item-ID", Body.ItemID.ToString("X4"));
            if (Body.Location == ItemChangeLocation.Armoury)
            {
                parsedValues.Add("Item-Pos", string.Format("{0} - {1}", Body.Location, (ItemChangeArmouryPosition)Body.Position));
            }
            else
            {
                parsedValues.Add("Item-Pos", string.Format("{0} - {1}", Body.Location, Body.Position));
            }

            parsedValues.Add("Item-Amount", Body.Amount.ToString());
            parsedValues.Add("Attributes", string.Format("{0:X2} {1:X2} {2:X4} {3:X4}", Body.Attribute1, Body.Attribute2, Body.Condition, Body.Spiritbond));

            parsedValues.Add("Glamour-ID", Body.GlamourID.ToString("X4"));
            parsedValues.Add("Unknown-1", string.Format("{0:X4} {1:X2} {2:X4} {3:X4} {4:X4} {5:X2}", 
                Body.Unknown0, Body.Unknown1, Body.Unknown2, Body.Unknown3, Body.Unknown4, Body.Unknown5));
            parsedValues.Add("Unknown-2", string.Format("{0:X4} {1:X4} {2:X4} {3:X4} {4:X4} {5:X4}",
                Body.Unknown6, Body.Unknown7, Body.Unknown8, Body.Unknown9, Body.Unknown10, Body.Unknown11));

            return this;
        }
    }
}
