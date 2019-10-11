using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PacketAnalyzer
{
    public class Character
    {
        static Dictionary<ulong, string> storage = new Dictionary<ulong, string>();
        static FileStream fs = null;
        const int packetLength = 2 + 4 + 32;
        public static async void Init(string path)
        {
            fs = File.Open(path, FileMode.OpenOrCreate);

            byte[] buffer = new byte[packetLength];
            while (await fs.ReadAsync(buffer, 0, packetLength) == packetLength)
            {
                ushort server = BitConverter.ToUInt16(buffer, 0);
                uint id = BitConverter.ToUInt32(buffer, 2);
                string name = Encoding.UTF8.GetString(buffer, 6, 32).TrimEnd((char)0);

                var key = getKey(server, id);
                if (storage.ContainsKey(key))
                {
                    storage[key] = name;
                } else
                {
                    storage.Add(key, name);
                }
                
            }
        }

        public static void AddToFile(ushort server, uint id, string name)
        {
            if (fs == null) return;

            // Save only players
            if ((id & 0x10000000) != 0x10000000) return;

            byte[] buffer = new byte[packetLength + 8];
            BitConverter.GetBytes(server).CopyTo(buffer, 0);
            BitConverter.GetBytes(id).CopyTo(buffer, 2);
            Encoding.UTF8.GetBytes(name).CopyTo(buffer, 6);

            fs.Write(buffer, 0, packetLength);
            fs.Flush(true);
        }

        private static ulong getKey(ushort server, uint id)
        {
            return ((ulong)server << 32) | id;
        }

        public static void Ensure(Network.FFXIVChinaServer server, uint id, string name)
        {
            Ensure((ushort)server, id, name);
        }
        public static void Ensure(ushort server, uint id, string name)
        {
            ulong key = getKey(server, id);
            if (storage.ContainsKey(key))
            {
                if (storage[key] == name) return;

                storage[key] = name;
            }
            else
            {
                storage.Add(key, name);
            }

            AddToFile(server, id, name);
        }

        public static string Get(ushort server, uint id)
        {
            ulong key = getKey(server, id);
            if (storage.TryGetValue(key, out var name))
            {
                return name;
            }
            else
            {
                return "(unknown)";
            }
        }

        public static string Dump()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Count: {0}\r\n", storage.Count);

            foreach (var pair in storage) {
                var server = Enum.GetName(typeof(Network.FFXIVChinaServer), (ushort)(pair.Key >> 32));
                sb.AppendFormat("{1:X8} <{0}> {2}\r\n", server, pair.Key & 0xFFFFFFFF, pair.Value);
            }

            return sb.ToString();
        }
    }
}
