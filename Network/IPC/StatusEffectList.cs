using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace PacketAnalyzer.Network.IPC
{
    /**
     * Packet 0151 (StatusEffectList)
						int curTP = (int)BitConverter.ToInt16(buffer, 16);
						int maxTP = 1000;
     * 0               4               8               12              16
     * +-------+-------+-------+-------+-------+-------+-------+-------+
     * | (?)           | HP Current    | HP Max        | MP Max| MP Cur|
     * +-------+-------+-------+-------+-------+-------+-------+-------+
     * | TP Cur| (?)   | B1 ID | Bi Ext| Duration      | ActorID       |
     * +-------+-------+-------+-------+-------+-------+-------+-------+
     * | B2 ...                                        | (?)           |
     * +-------+-------+-------+-------+-------+-------+-------+-------+
     */

    [StructLayout(LayoutKind.Explicit)]
    struct StatusEffectListHeader
    {
        [FieldOffset(0)]
        public uint Unknown0;

        [FieldOffset(4)]
        public uint CurrentHP;

        [FieldOffset(8)]
        public uint MaxHP;

        [FieldOffset(12)]
        public ushort MaxMP;

        [FieldOffset(14)]
        public ushort CurrentMP;

        [FieldOffset(16)]
        public ushort CurrentTP;

        [FieldOffset(18)]
        public ushort Unknown1;
    }

    [StructLayout(LayoutKind.Explicit)]
    struct StatusEffectListItem
    {
        [FieldOffset(0)]
        public ushort StatusID;

        [FieldOffset(2)]
        public ushort StatusExtra;

        [FieldOffset(4)]
        public float Duration;

        [FieldOffset(8)]
        public uint ActorID;
    }

    class StatusEffectList : IPCBase
    {
        public StatusEffectListHeader Header;
        public StatusEffectListItem[] Items;

        public StatusEffectList(byte[] message, int offset)
        {
            int headerLength = PacketParser.ParsePacket(message, offset, out Header);

            Items = new StatusEffectListItem[(message.Length - headerLength - 4) / 12];
            for (int i = 0; i < Items.Length; ++i)
            {
                PacketParser.ParsePacket(message, headerLength + 12 * i, out Items[i]);
            }
            Dump = false;
        }

        public StatusEffectList WriteParams(Dictionary<string, string> parsedValues)
        {
            parsedValues.Add("User-Status", string.Format("{0}/{1} {2}/{3} {4}/{5}", Header.CurrentHP, Header.MaxHP, Header.CurrentMP, Header.MaxMP, Header.CurrentTP, 1000));
            parsedValues.Add("Unknown", string.Format("{0:X8} {1:X4}", Header.Unknown0, Header.Unknown1));

            for (int i = 0; i < Items.Length; ++i)
            {
                var item = Items[i];
                parsedValues.Add(string.Format("Item-{0}", i), string.Format("{3:X8} - {0:X4}:{1:X4} {2}s", 
                    item.StatusID, item.StatusExtra, item.Duration, item.ActorID));
            }

            return this;
        }
    }
}
