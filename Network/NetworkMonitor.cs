namespace PacketAnalyzer
{
    interface NetworkMonitor
    {
        void HandleNetworkMessageReceived(string connection, long epoch, byte[] message);
        void HandleNetworkMessageSent(string connection, long epoch, byte[] message);
    }
}