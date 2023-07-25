using PubnubApi;
using PubnubApi.Unity;
using NubEval.Game.Networking;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace NubEval
{
    public class VirtualDevice : MonoBehaviour
    {
        [SerializeField] private PNConfigDataAsset configAsset;
        [SerializeField] private PNDevice networkDevice;
        [SerializeField] private PlayerPrefsAsset devicePlayerPrefs;
        [SerializeField] private TextMeshProUGUI title;

        private SubscribeCallbackListener _listener;

        public void Construct(SubscribeCallbackListener listener)
        {
            _listener = listener;
            networkDevice = new PNDevice(configAsset.Data, devicePlayerPrefs.PnUserID, devicePlayerPrefs.DeviceData);
            networkDevice.SetListener(_listener);
        }

        private void OnInputSetAccountData()
        {

        }

        public async void OnInputConnect()
        {
            await networkDevice.Connect();

            var accoutData = await networkDevice.UserData.GetAccountDataAsync(devicePlayerPrefs.PnUserID);
            title.text = accoutData.Item2.DisplayName;
        }

        public async void OnInputJoin()
        {
            await JoinLobby();
        }

        public async void OnInputDisconnect()
        {
            await networkDevice.Disconnect();
            title.text = "-disconnected-";
        }

        public void OnUserJoin(UserId user, UserAccountData accountData)
        {

        }

        private async Task JoinLobby()
        {
            List<PresenceState> states = new List<PresenceState>
            {
                new PresenceState(StateType.lobbyState, "Online"),
                new PresenceState(StateType.matchState, "Idle")
            };

            await networkDevice.Presence.SetPresenceState(Channels.DebugChannel, states);
            //networkDevice.RemoteEventsLobby.SubscribeToLobbyEvents(this);
        }

        //void ILobbyEventsHandler.OnUserJoin(UserId user, UserAccountData accountData)
        //{
        //    Debug.LogWarning($"Joined: {accountData.DisplayName}");
        //}

        //void ILobbyEventsHandler.OnUserLeave(UserId user, UserAccountData accountData)
        //{
        //    Debug.LogWarning($"Left: {user}");
        //}


        private void OnDestroy()
        {
            networkDevice?.Dispose();
        }
    }
}
