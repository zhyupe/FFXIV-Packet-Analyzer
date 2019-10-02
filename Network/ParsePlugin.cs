using Advanced_Combat_Tracker;
using FFXIV_ACT_Plugin;
using FFXIV_ACT_Plugin.Memory;
using System;

namespace PacketAnalyzer.Network
{
    class ParsePlugin
    {
        private readonly FFXIV_ACT_Plugin.FFXIV_ACT_Plugin _parsePlugin;
        private dynamic _oldReceivedHandler;
        private dynamic _oldSentHandler;

        private bool _callingReceived;
        private bool _callingSent;

        public NetworkMonitor Network { private get; set; }

        public ParsePlugin(IActPluginV1 plugin)
        {
            _parsePlugin = (FFXIV_ACT_Plugin.FFXIV_ACT_Plugin)plugin;
        }

        private Memory Memory => _parsePlugin.GetFieldValue<dynamic>("_Memory");
        public MemoryScanSettings MemoryScanSettings => Memory?.GetFieldValue<dynamic>("_config");
        public ScanCombatants ScanCombatants => MemoryScanSettings?.GetFieldValue<dynamic>("ScanCombatants");
        public ScanPackets ScanPackets => MemoryScanSettings?.GetFieldValue<dynamic>("ScanPackets");
        public dynamic Monitor => ScanPackets?.GetFieldValue<dynamic>("_monitor");

        public SettingsPropertyPage Settings => _parsePlugin.Settings;

        public string GetPlayerName()
        {
            return GetPlayer()?.Name;
        }

        public string GetPlayerWorldName()
        {
            return GetPlayer()?.WorldName;
        }

        public int GetPlayerWorldId()
        {
            return GetPlayer()?.WorldID ?? 0;
        }

        public Combatant GetPlayer()
        {
            var list = ScanCombatants?.GetCombatantList();
            if (list == null) return null;
            if (list.Count < 1) return null;
            return list[0];
        }

        public void Start()
        {
            Settings.MemoryScanSettingsChanged += HandleMemoryScanSettingsChanged;
            HookParseNetwork();
        }

        public void Stop()
        {
            if (Settings != null) Settings.MemoryScanSettingsChanged -= HandleMemoryScanSettingsChanged;
        }

        private void HandleMemoryScanSettingsChanged(object sender, MemoryScanSettings e)
        {
            HookParseNetwork();
        }

        private void HookParseNetwork()
        {
            if (Monitor == null) return;

            _oldReceivedHandler = Monitor.MessageReceived;
            Type rDelegateType = Monitor.MessageReceived.GetType();
            dynamic messageReceived = Delegate.CreateDelegate(rDelegateType, this, "HandleMessageReceived");
            Monitor.MessageReceived = messageReceived;

            _oldSentHandler = Monitor.MessageSent;
            Type sDelegateType = Monitor.MessageSent.GetType();
            dynamic messageSent = Delegate.CreateDelegate(sDelegateType, this, "HandleMessageSent");
            Monitor.MessageSent = messageSent;
        }

        private void HandleMessageSent(string connection, long epoch, byte[] message)
        {
            if (_callingSent) return;
            _callingSent = true;
            _oldSentHandler?.Invoke(connection, epoch, message);
            Network?.HandleNetworkMessageSent(connection, epoch, message);
            _callingSent = false;
        }

        private void HandleMessageReceived(string connection, long epoch, byte[] message)
        {
            if (_callingReceived) return;
            _callingReceived = true;
            _oldReceivedHandler?.Invoke(connection, epoch, message);
            Network?.HandleNetworkMessageReceived(connection, epoch, message);
            _callingReceived = false;
        }
    }
}
