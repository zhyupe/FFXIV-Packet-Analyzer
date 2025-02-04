﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace PacketAnalyzer.Network
{
    public enum PacketParseResult : int
    {
        /// Buffer is too short to dissect a message.
        Incomplete = -1,

        /// Invalid data detected.
        Malformed = -2
    };

    public class ParseException : Exception
    {
        private readonly PacketParseResult _result;

        public ParseException(string message, PacketParseResult reason) : base(message)
        {
            _result = reason;
        }

        public override string ToString()
        {
            return $"{Message} - {_result}.\n{StackTrace}";
        }
    }

    public class PacketParser
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="output"></param>
        /// <returns>
        /// How many bytes should be skipped.
        /// </returns>
        public static int ParsePacket<T>(byte[] buffer, int offset, out T output) where T : struct
        {
            var size = Marshal.SizeOf(typeof(T));

            if (buffer.Length - offset < size)
            {
                throw new ParseException("ParsePacket failed", PacketParseResult.Incomplete);
            }

            output = Util.ByteArrayToStructure<T>(buffer, offset);

            return size;
        }

        public static IPCBase ParseIPCPacket(ServerZoneIpcType type,byte[] message, int offset, Dictionary<string, string> parsedValues)
        {
            switch (type)
            {
                case ServerZoneIpcType.Announcement:
                    return new IPC.Announcement(message, offset).WriteParams(parsedValues);
                case ServerZoneIpcType.CompanyBoard:
                    return new IPC.CompanyBoard(message, offset).WriteParams(parsedValues);
                case ServerZoneIpcType.GroupMessage:
                    return new IPC.GroupMessage(message, offset).WriteParams(parsedValues);
                case ServerZoneIpcType.PublicMessage:
                    return new IPC.PublicMessage(message, offset).WriteParams(parsedValues);
                case ServerZoneIpcType.Ability1:
                case ServerZoneIpcType.Ability8:
                case ServerZoneIpcType.Ability16:
                case ServerZoneIpcType.Ability24:
                case ServerZoneIpcType.Ability32:
                    return new IPC.Ability(type, message, offset).WriteParams(parsedValues);
                case ServerZoneIpcType.AddStatusEffect:
                    return new IPC.AddStatusEffect(message, offset).WriteParams(parsedValues);
                case ServerZoneIpcType.StatusEffectList:
                    return new IPC.StatusEffectList(message, offset).WriteParams(parsedValues);
                case ServerZoneIpcType.ItemInit:
                    return new IPC.ItemInit(message, offset).WriteParams(parsedValues);
                case ServerZoneIpcType.ItemChange:
                    return new IPC.ItemChange(message, offset).WriteParams(parsedValues);
                case ServerZoneIpcType.ItemSimple:
                    return new IPC.ItemSimple(message, offset).WriteParams(parsedValues);
                case ServerZoneIpcType.ActorControl143:
                    return new IPC.ActorControl143(message, offset).WriteParams(parsedValues);
                case ServerZoneIpcType.ActorMove:
                case (ServerZoneIpcType)0x0145:
                    return new IPCIgnore();
                default:
                    parsedValues.Add("Data", "(unknown)");
                    return null;
            }
        }
    }
}
