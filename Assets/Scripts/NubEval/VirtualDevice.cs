using PubnubApi;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace NubEval
{
    public class VirtualDevice : MonoBehaviour,
        ILobbyEventsHandler
    {
        [SerializeField] private PNConfigDataAsset configAsset;
        [SerializeField] private PNDevice networkDevice;
        [SerializeField] private PlayerPrefsAsset devicePlayerPrefs;

        private CancellationTokenSource cts = new CancellationTokenSource();

        private void Awake()
        {
            networkDevice = new PNDevice(configAsset.Data, devicePlayerPrefs.PnUserID, devicePlayerPrefs.DeviceData);
        }

        private void OnInputSetAccountData()
        {

        }

        public async void OnInputConnect()
        {
            cts.CancelAfter(5000); //cancel trying to connect fter 5 sec
            await networkDevice.Connection.Connect(cts.Token);

            networkDevice.RemoteEventsLobby.SubscribeLobbyEvents(this);
            await networkDevice.Presence.SetPresenceState(Channels.DebugChannel, new PresenceState("lobbyState", "Poking nose"));
        }

        public void OnInputDisconnect()
        {
            networkDevice.Connection.Disconnect();
        }

        private void OnDisable()
        {
            cts.Cancel();
            networkDevice?.Dispose();    
        }

        public void OnUserJoined(UserId user, UserAccountData accountData)
        {
            Debug.LogWarning($"Joined: {accountData.DisplayName}");
        }
    }
}
