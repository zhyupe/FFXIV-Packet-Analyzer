using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;
using Advanced_Combat_Tracker;

namespace PacketAnalyzer.Network
{

    public static class Util
    {

        private static readonly long _dt1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;

        public static T ByteArrayToStructure<T>(byte[] bytes, int offset = 0, Endianness endianness = Endianness.LittleEndian) where T : struct
        {
            MaybeAdjustEndianness(typeof(T), bytes, endianness, offset);

            T stuff;
            var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            try
            {
                stuff = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject() + offset, typeof(T));
            }
            finally
            {
                handle.Free();
            }
            return stuff;
        }

        public enum Endianness
        {
            BigEndian,
            LittleEndian
        }

        public static void MaybeAdjustEndianness(Type type, byte[] data, Endianness endianness, int startOffset = 0)
        {
            if ((BitConverter.IsLittleEndian) == (endianness == Endianness.LittleEndian))
            {
                // nothing to change => return
                return;
            }

            foreach (var field in type.GetFields())
            {
                var fieldType = field.FieldType;
                if (field.IsStatic)
                    // don't process static fields
                    continue;

                if (fieldType == typeof(string))
                    // don't swap bytes for strings
                    continue;

                var offset = Marshal.OffsetOf(type, field.Name).ToInt32();

                // handle enums
                if (fieldType.IsEnum)
                    fieldType = Enum.GetUnderlyingType(fieldType);

                // check for sub-fields to recurse if necessary
                var subFields = fieldType.GetFields().Where(subField => subField.IsStatic == false).ToArray();

                var effectiveOffset = startOffset + offset;

                if (subFields.Length == 0)
                {
                    Array.Reverse(data, effectiveOffset, Marshal.SizeOf(fieldType));
                }
                else
                {
                    // recurse
                    MaybeAdjustEndianness(fieldType, data, endianness, effectiveOffset);
                }
            }
        }

        public static long EpochMillis(this DateTime t)
        {
            var unixTimestamp = t.ToUniversalTime().Ticks - _dt1970;
            unixTimestamp /= TimeSpan.TicksPerMillisecond;
            return unixTimestamp;
        }
        public static string BuildHexDump(byte[] bytes, int offset = 0)
        {
            StringBuilder sb = new StringBuilder();
            int lines = (int)Math.Ceiling((bytes.Length - offset) / 16.0f);
            sb.AppendLine("#ADDRESS: 00 01 02 03 04 05 06 07   08 09 0A 0B 0C 0D 0E 0F : 0123456789ABCDEF");
            sb.AppendLine("==============================================================================");
            for (int i = 0; i < lines; i++)
            {
                sb.Append(string.Format("{0:X8}: ", i * 16));
                for (int j = 0; j < 8; j++)
                {
                    int l = i * 16 + j + offset;
                    if (bytes.Length > l)
                        sb.Append(string.Format("{0:X2} ", bytes[l]));
                    else
                        sb.Append("   ");
                }
                sb.Append("  ");
                for (int j = 8; j < 16; j++)
                {
                    int l = i * 16 + j + offset;
                    if (bytes.Length > l)
                        sb.Append(string.Format("{0:X2} ", bytes[l]));
                    else
                        sb.Append("   ");
                }

                sb.Append(": ");
                for (int j = 0; j < 16; j++)
                {
                    int l = i * 16 + j + offset;
                    if (bytes.Length > l)
                        sb.Append(GetPrintableChar(bytes[l]));
                    else
                        sb.Append(" ");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        private static string GetPrintableChar(byte p)
        {
            if (p >= 32 && p <= 126)
                return ((char)p).ToString();
            else
                return ".";
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2} ", b);
            return hex.ToString().TrimEnd();
        }

        public static bool IsElevated
        {
            get
            {
                var id = WindowsIdentity.GetCurrent();
                return id.Owner != id.User;
            }
        }
    }
}
