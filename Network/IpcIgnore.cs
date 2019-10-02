using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketAnalyzer.Network
{
    class IPCIgnore : IPCBase
    {
        public IPCIgnore()
        {
            Dump = false;
            Display = false;
        }
    }
}
