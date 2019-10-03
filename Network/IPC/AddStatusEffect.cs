using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace PacketAnalyzer.Network.IPC
{
    /**
     * Packet 0141 (AddStatusEffect)
     * 0               4               8               12              16
     * +-------+-------+-------+-------+-------+-------+-------+-------+
     * | LastBuffPktID | User ID       | (?)   | (?)   | HP Current    |
     * +-------+-------+-------+-------+-------+-------+-------+-------+
     * | MP Cur| TP Cur| HP Max        | MP Max| C | ? | (?)   | B1 ID |
     * +-------+-------+-------+-------+-------+-------+-------+-------+
     * | Bi Ext| (?)   | B1 Duration   | B1 ActorID    | (?)   | B2 ID | ...
     * +-------+-------+-------+-------+-------+-------+-------+-------+
     */

    [StructLayout(LayoutKind.Explicit)]
    struct AddStatusEffectHeader
    {
        [FieldOffset(0)]
        public uint LastBuffPacketID;

        [FieldOffset(4)]
        public uint UserID;

        [FieldOffset(8)]
        public ushort Unknown1;

        [FieldOffset(10)]
        public ushort Unknown2;

        [FieldOffset(12)]
        public uint CurrentHP;

        [FieldOffset(16)]
        public ushort CurrentMP;

        [FieldOffset(18)]
        public ushort CurrentTP;

        [FieldOffset(20)]
        public uint MaxHP;

        [FieldOffset(24)]
        public ushort MaxMP;

        [FieldOffset(25)]
        public byte Count;

        [FieldOffset(26)]
        public byte Unknown3;

        [FieldOffset(28)]
        public ushort Unknown4;
    }

    [StructLayout(LayoutKind.Explicit)]
    struct AddStatusEffectItem
    {
        [FieldOffset(0)]
        public ushort StatusID;

        [FieldOffset(2)]
        public ushort StatusExtra;

        [FieldOffset(4)]
        public ushort Unknown1;

        [FieldOffset(6)]
        public float Duration;

        [FieldOffset(10)]
        public uint ActorID;

        [FieldOffset(14)]
        public ushort Unknown2;
    }

    class AddStatusEffect : IPCBase
    {
        public AddStatusEffectHeader Header;
        public AddStatusEffectItem[] Items;

        public AddStatusEffect(byte[] message, int offset)
        {
            int headerLength = 30;
            PacketParser.ParsePacket(message, offset, out Header);

            Items = new AddStatusEffectItem[Header.Count];
            for (int i = 0; i < Header.Count; ++i)
            {
                PacketParser.ParsePacket(message, headerLength + 16 * i, out Items[i]);
            }
        }

        public AddStatusEffect WriteParams(Dictionary<string, string> parsedValues)
        {
            parsedValues.Add("LastBuffPacketID", Header.LastBuffPacketID.ToString());
            parsedValues.Add("User-Id", string.Format("{0:X8}", Header.UserID));
            parsedValues.Add("User-Status", string.Format("{0}/{1} {2}/{3} {4}/{5}", Header.CurrentHP, Header.MaxHP, Header.CurrentMP, Header.MaxMP, Header.CurrentTP, 1000));
            parsedValues.Add("Unknown", string.Format("{0:X4} {1:X4} {2:X2} {3:X4}", Header.Unknown1, Header.Unknown2, Header.Unknown3, Header.Unknown4));


            for (int i = 0; i < Header.Count; ++i)
            {
                var item = Items[i];
                parsedValues.Add(string.Format("Item-{0}", i), string.Format("{4:X8} - {0:X4}:{1:X4} {3}s ? {2:X4} {5:X4}", 
                    item.StatusID, item.StatusExtra, item.Unknown1, item.Duration, item.ActorID, item.Unknown2));
            }

            return this;
        }
    }
}
