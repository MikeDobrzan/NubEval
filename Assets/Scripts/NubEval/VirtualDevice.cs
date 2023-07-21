using PubnubApi;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace NubEval
{
    public class VirtualDevice : MonoBehaviour,
        ILobbyEventsHandler
    {
        [SerializeField] private PNConfigDataAsset configAsset;
        [SerializeField] private PNDevice networkDevice;
        [SerializeField] private PlayerPrefsAsset devicePlayerPrefs;
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private PNDevice device;

        private CancellationTokenSource cts = new CancellationTokenSource();

        private void Start()
        {
            
        }

        private void OnInputSetAccountData()
        {

        }

        public async void OnInputConnect()
        {
            //networkDevice = new PNDevice(configAsset.Data, devicePlayerPrefs.PnUserID, devicePlayerPrefs.DeviceData);

            cts.CancelAfter(5000); //cancel trying to connect fter 5 sec
            await networkDevice.Connection.Connect(cts.Token);

            var accoutData = await networkDevice.UserData.GetAccountDataAsync(devicePlayerPrefs.PnUserID);
            title.text = accoutData.Item2.DisplayName;
        }

        public async void OnInputJoin()
        {
            await JoinLobby();
        }

        public void OnInputDisconnect()
        {
            networkDevice.Connection.Disconnect();
            title.text = "-disconnected-";
        }

        private void OnDisable()
        {
            cts.Cancel();
        }

        public void OnUserJoin(UserId user, UserAccountData accountData)
        {

        }

        private async Task JoinLobby()
        {
            await networkDevice.Presence.SetPresenceState(Channels.DebugChannel, new PresenceState("lobbyState", "In Lobby"));
            networkDevice.RemoteEventsLobby.SubscribeLobbyEvents(this);
        }

        void ILobbyEventsHandler.OnUserJoin(UserId user, UserAccountData accountData)
        {
            Debug.LogWarning($"Joined: {accountData.DisplayName}");
        }

        void ILobbyEventsHandler.OnUserLeave(UserId user, UserAccountData accountData)
        {
            Debug.LogWarning($"Left: {user}");
        }
    }
}
