using Advanced_Combat_Tracker;
using PacketAnalyzer.Network;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PacketAnalyzer
{
    public static class DumpManager
    {
        const int maxPacketSize = 65535;
        static string dumpDir = Path.Combine();
        static Thread pcapLogger = null;

        // Global Header | Packet Header | Packet Data | Packet Header | Packet Data | Packet Header | Packet Data | ...
        /**
         * @url: https://wiki.wireshark.org/Development/LibpcapFileFormat
         */
        [StructLayout(LayoutKind.Sequential)]
        struct PcapGlobalHeader
        {
            public PcapGlobalHeader(uint network)
            {
                // 147 is valid for private use, see http://www.tcpdump.org/linktypes.html
                magic_number = 0xff14ff14;
                version_major = 2;
                version_minor = 4;
                thiszone = 0;
                sigfigs = 0;
                snaplen = maxPacketSize;
                this.network = network;
            }

            /* magic number */
            public uint magic_number;
            /* major version number */
            public ushort version_major;
            /* minor version number */
            public ushort version_minor;
            /* GMT to local correction */
            public int thiszone;
            /* accuracy of timestamps */
            public uint sigfigs;
            /* max length of captured packets, in octets */
            public uint snaplen;
            /* data link type */
            public uint network;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct PcapPacketHeader
        {
            public PcapPacketHeader(uint incl_len, uint orig_len)
            {
                var now = DateTime.Now;
                ts_sec = (uint)(now.EpochMillis() / 1000);
                ts_usec = (uint)((now.Ticks / 10) % 1e6);
                this.incl_len = incl_len;
                this.orig_len = orig_len;
            }

            /* timestamp seconds */
            public uint ts_sec;
            /* timestamp microseconds */
            public uint ts_usec;
            /* number of octets of packet saved in file */
            public uint incl_len;
            /* actual length of packet */
            public uint orig_len;
        }

        static volatile BlockingCollection<byte[]> pcapQueue = new BlockingCollection<byte[]>();
        static void PcapLogger()
        {
            string dumpPath = Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, "PacketDump", DateTime.UtcNow.EpochMillis() + ".pcap");
            int flushCount = 0;

            try
            {
                using (FileStream fs = File.OpenWrite(dumpPath))
                {
                    var globalHeader = Network.Util.StructureToByteArray(new PcapGlobalHeader(147));
                    fs.Write(globalHeader, 0, globalHeader.Length);

                    foreach (byte[] buffer in pcapQueue.GetConsumingEnumerable())
                    {
                        try
                        {
                            int dumpLength = buffer.Length > maxPacketSize ? maxPacketSize : buffer.Length;

                            var packetHeader = Network.Util.StructureToByteArray(new PcapPacketHeader((uint)dumpLength, (uint)buffer.Length));
                            fs.Write(packetHeader, 0, packetHeader.Length);
                            fs.Write(buffer, 0, dumpLength);

                            if (++flushCount > 15)
                            {
                                flushCount = 0;
                                fs.Flush(true);
                            }
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(string.Format("[E][PcapLogger: {0}]{1}\r\n{2}", e.GetType(), e.Message, e.StackTrace));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(string.Format("[E][Failed starting PcapLogger: {0}]{1}\r\n{2}", e.GetType(), e.Message, e.StackTrace));
            }

            pcapLogger = null;
        }

        public static void FlushPcap()
        {
            pcapQueue.CompleteAdding();
            pcapQueue = new BlockingCollection<byte[]>();
        }

        public static void Pcap(byte[] buffer)
        {
            if (pcapLogger == null)
            {
                pcapLogger = new Thread(PcapLogger);
                pcapLogger.Start();
            }

            pcapQueue.Add(buffer);
        }

        public async static Task LoadPcap(string path, Action<byte[], DateTime> callback, IProgress<float> progress = null)
        {
            byte[] headerBuffer = new byte[16];
            int bytesRead;
            using (FileStream fs = File.OpenRead(path))
            {
                // Skip global header
                fs.Seek(24, SeekOrigin.Begin);

                while ((bytesRead = await fs.ReadAsync(headerBuffer, 0, 16)) == 16)
                {
                    PacketParser.ParsePacket(headerBuffer, 0, out PcapPacketHeader header);
                    if (header.incl_len > maxPacketSize) return;

                    byte[] message = new byte[header.incl_len];
                    bytesRead = await fs.ReadAsync(message, 0, (int)header.incl_len);

                    if (bytesRead != header.incl_len) return;
                    callback(message, DateTimeOffset.FromUnixTimeSeconds(header.ts_sec).DateTime.AddTicks(header.ts_usec * 10));

                    if (progress != null)
                    {
                        progress.Report((float)fs.Position * 100 / fs.Length);
                    }
                }
            }
        }

        public static string Dump(Dictionary<string, string> param, byte[] buffer, int offset)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in param)
            {
                sb.Append(item.Key);
                sb.Append(": ");
                sb.Append(item.Value);
                sb.AppendLine();
            }

            if (offset >= 0)
            {
                string hexDump = Util.BuildHexDump(buffer, offset);
                sb.Append(hexDump);
            }

            return sb.ToString();
        }

        static async Task WriteTextAsync(string filePath, string text)
        {
            byte[] encodedText = Encoding.UTF8.GetBytes(text);

            using (FileStream sourceStream = new FileStream(filePath,
                FileMode.Append, FileAccess.Write, FileShare.None,
                bufferSize: 4096, useAsync: true))
            {
                await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
            };
        }

        public static Task Write(Dictionary<string, string> param, string content)
        {
            string dumpPath = Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, "PacketDump", param["ID"] + ".txt");
            return WriteTextAsync(dumpPath, content);
        }

        public static string Write(Dictionary<string, string> param, byte[] buffer, int offset)
        {
            string dump = Dump(param, buffer, offset);
            Write(param, dump);
            return dump;
        }
    }
}
