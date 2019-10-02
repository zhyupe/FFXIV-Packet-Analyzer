using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace PacketAnalyzer.Network.IPC
{
    /**
     * Packet 0154 0157 0158 0159 015A - Ability
     * 0               4               8               12              16
     * +-------+-------+-------+-------+-------+-------+-------+-------+
     * | Actor         | Unknown0      | ActionID      | Index         |
     * +-------+-------+-------+-------+-------+-------+-------+-------+
     * | (?)                                                   | AOff  |
     * +-------+-------+-------+-------+-------+-------+-------+-------+
     * |   | C | (?)                           | 1-1-A         | 1-1-B |
     * +-------+-------+-------+-------+-------+-------+-------+-------+
     * | ...   | 1-16-A        | 1-16-B        | 2-1-A         | 2-1-B |
     * +-------+-------+-------+-------+-------+-------+-------+-------+
     * | ...   | 2-16-A        | 2-16-B        | (?)                   |
     * +-------+-------+-------+-------+-------+-------+-------+-------+
     * | Target1       | Target1 Args  | Target2       | Target2 Args  |
     * +-------+-------+-------+-------+-------+-------+-------+-------+
     */

    [StructLayout(LayoutKind.Explicit)]
    struct AbilityHeader
    {
        [FieldOffset(0)]
        public uint Actor;

        [FieldOffset(4)]
        public uint Unknown0;

        [FieldOffset(8)]
        public uint ActionID;

        [FieldOffset(12)]
        public uint Index;

        [FieldOffset(16)]
        public uint Unknown1;

        [FieldOffset(20)]
        public uint Unknown2;

        [FieldOffset(24)]
        public uint Unknown3;

        [FieldOffset(28)]
        public uint Unknown4;

        [FieldOffset(32)]
        public byte Unknown5;

        [FieldOffset(33)]
        public byte Count;

        [FieldOffset(34)]
        public ushort Unknown6;

        [FieldOffset(36)]
        public uint Unknown7;

        [FieldOffset(40)]
        public ushort Unknown8;
    }

    enum AbilityTargetArgumentType : byte
    {
        None = 0,
        Buff1 = 0x0F,
        Buff2 = 0x10,
        Buff3 = 0x11,
    }

    class AbilityTargetArguments
    {
        public AbilityTargetArguments(byte[] message, int offset)
        {
            Type = (AbilityTargetArgumentType)message[offset];
            Unknown0 = message[offset + 1];
            Unknown1 = BitConverter.ToUInt16(message, offset + 2);
            Unknown2 = BitConverter.ToUInt16(message, offset + 4);
            StatusID = BitConverter.ToUInt16(message, offset + 6);
        }

        public AbilityTargetArgumentType Type;
        public byte Unknown0;
        public ushort Unknown1;
        public ushort Unknown2;
        public ushort StatusID;

        public override string ToString()
        {
            return string.Format("{4} {0:X2} {1:X4} {2:X4}, Status: {3:X4}", Unknown0, Unknown1, Unknown2, StatusID, Type);
        }
    }

    class AbilityTarget {
        public uint ID;
        public uint Unknown0;

        public AbilityTargetArguments[] Arguments;

        public void ParseArguments(byte[] message, int offset)
        {
            Arguments = new AbilityTargetArguments[8];

            for (int i = 0; i < 8; ++i)
            {
                Arguments[i] = new AbilityTargetArguments(message, offset + 8 * i);
            }
        }
    }

    class Ability : IPCBase
    {
        ServerZoneIpcType Type;
        public AbilityHeader Header;

        public uint Unknown8;
        public ushort Unknown9;

        public AbilityTarget[] Target;

        public int MaxCount()
        {
            switch (Type) {
                case ServerZoneIpcType.Ability1:
                    return 1;
                case ServerZoneIpcType.Ability8:
                    return 8;
                case ServerZoneIpcType.Ability16:
                    return 16;
                case ServerZoneIpcType.Ability24:
                    return 24;
                case ServerZoneIpcType.Ability32:
                    return 32;
            }

            throw new Exception(string.Format("Unexcepted IPC Type {0}", Type));
        }

        public Ability(ServerZoneIpcType Type, byte[] message, int offset)
        {
            this.Type = Type;
            int headerLength = 42;
            PacketParser.ParsePacket(message, offset, out Header);

            int maxCount = MaxCount();
            int count = Header.Count > maxCount ? maxCount : Header.Count;

            int targetOffset = offset + headerLength + 4 * 16 * maxCount + 6;
            Unknown8 = BitConverter.ToUInt32(message, targetOffset - 6);
            Unknown9 = BitConverter.ToUInt16(message, targetOffset - 2);

            Target = new AbilityTarget[count];
            for (int i = 0; i < count; ++i)
            {
                var targetItem = new AbilityTarget();
                targetItem.ID = BitConverter.ToUInt32(message, targetOffset + i * 2 * 4);
                targetItem.Unknown0 = BitConverter.ToUInt32(message, targetOffset + i * 2 * 4 + 4);
                targetItem.ParseArguments(message, headerLength + 4 * 16 * i);
                Target[i] = targetItem;
            }
        }

        public Ability WriteParams(Dictionary<string, string> parsedValues)
        {
            parsedValues.Add("Ability-Index", Header.Index.ToString());
            parsedValues.Add("Ability-Actor", string.Format("{0:X8}", Header.Actor));
            parsedValues.Add("Ability-Action", string.Format("{0:X8}", Header.ActionID));
            parsedValues.Add("Header-Unknown", string.Format("{0:X8} {1:X8} {2:X8} {3:X8} {4:X8} {5:X2} {6:X4} {7:X8} {8:X4}", 
                Header.Unknown0, Header.Unknown1, Header.Unknown2, Header.Unknown3, Header.Unknown4, Header.Unknown5, Header.Unknown6, Header.Unknown7, Header.Unknown8));
            parsedValues.Add("Target-Count", Header.Count.ToString());

            for (int i = 0; i < Target.Length; ++i)
            {
                parsedValues.Add(string.Format("Target-{0}", i), string.Format("{0:X8} | {1:X8}", Target[i].ID, Target[i].Unknown0, Target[i].ID));
                for (int j = 0; j < Target[i].Arguments.Length; ++j)
                {
                    parsedValues.Add(string.Format("Target-{0}-{1}", i, j), Target[i].Arguments[j].ToString());
                }
            }
            
            return this;
        }
    }
}
