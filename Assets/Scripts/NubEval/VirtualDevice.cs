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

        private CancellationTokenSource cts = new CancellationTokenSource();

        private void Awake()
        {
            networkDevice = new PNDevice(configAsset.Data, devicePlayerPrefs.PnUserID, devicePlayerPrefs.DeviceData);
        }

        private void Start()
        {
            
        }

        private void OnInputSetAccountData()
        {

        }

        public async void OnInputConnect()
        {
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
        }

        private void OnDisable()
        {
            cts.Cancel();
            networkDevice?.Dispose();    
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
