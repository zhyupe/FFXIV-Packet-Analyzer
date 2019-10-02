using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PacketAnalyzer.Network
{

    class FFXIVNetworkMonitor : NetworkMonitor
    {
        public void HandleNetworkMessageReceived(string connection, long epoch, byte[] message)
        {
            try
            {
                fireReceiveEvent(connection, epoch, message);
            }
            catch (Exception e)
            {
                try
                {
                    fireException(e);
                }
                catch { }
            }
        }
        public void HandleNetworkMessageSent(string connection, long epoch, byte[] message)
        {
            try
            {
                fireSendEvent(connection, epoch, message);
            }
            catch (Exception e)
            {
                try
                {
                    fireException(e);
                }
                catch { }
            }
        }

        public delegate void ExceptionHandler(Exception e);
        public event ExceptionHandler onException;
        private void fireException(Exception e)
        {
            onException?.Invoke(e);
        }

        public delegate void EventHandler(string connection, long epoch, byte[] message);
        public event EventHandler onReceiveEvent;
        public event EventHandler onSendEvent;

        private void fireReceiveEvent(string connection, long epoch, byte[] message)
        {
            onReceiveEvent?.Invoke(connection, epoch, message);
        }

        private void fireSendEvent(string connection, long epoch, byte[] message)
        {
            onSendEvent?.Invoke(connection, epoch, message);
        }
    }
}