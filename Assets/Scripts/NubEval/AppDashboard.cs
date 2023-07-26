using NubEval.Networking.Payloads;
using PubnubApi;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace NubEval
{
    public class AppDashboard : MonoBehaviour
    {
        [SerializeField] private AppDashboardDataAsset mockData;

        private PNDevice _mainDevice;

        public void Construct(PNDevice device)
        {
            _mainDevice = device;
        }

        public async void OnBtnConnect()
        {
            if (_mainDevice == null)
                return;

            Debug.Log("Connecting...");
            await _mainDevice.Connect();
            Debug.Log("Connected");
        }

        public async void OnBtnDisconnect()
        {
            if (_mainDevice == null)
                return;

            Debug.Log("Disconnecting...");
            await _mainDevice.Disconnect();
            Debug.Log("Disconnected");
        }

        public async void OnBtnSubscribeDebug()
        {
            if (_mainDevice == null)
                return;

            await _mainDevice.Presence.SubscribePresence(Channels.DebugChannel);            
        }

        public async void OnBtnSetPresencState()
        {
            List<PresenceState> states = new List<PresenceState>
            {
                new PresenceState(StateType.lobbyState, "Online"),
                new PresenceState(StateType.matchState, "Idle")
            };

            await _mainDevice.Presence.SetPresenceState(Channels.DebugChannel, states);
        }

        public async void OnButtonLeaveLobby()
        {
            if (_mainDevice == null)
                return;

            await _mainDevice.Presence.LeaveChannel(Channels.DebugChannel);
        }

        public async void OnBtnGetUsersInDebug()
        {
            if (_mainDevice == null)
                return;


            Debug.Log("Getting users...");

            var users = await _mainDevice.Presence.GetUserIDsInChannel(Channels.DebugChannel);

            string msg = $"users: ";

            foreach (var user in users)
            {
                msg += $" | {user}";
            }

            Debug.Log(msg);
        }

        public async void OnBtnAnnounceMatch()
        {

            //create match channel (with presence)

            Channel matchChannel = mockData.MatchAnnouncemet.MatchConfig.Channel;

            await _mainDevice.Subscriptions.SubscribeChannels(matchChannel);
            await _mainDevice.MetadataChannels.SetDefaultCustomData(matchChannel, mockData.MatchAnnouncemet.MatchConfig);
            await Task.Delay(2000); //give it some time and broadcast announcement //to debounce announcements
            await _mainDevice.MessageDispatcher.SendMsg(mockData.MatchAnnouncemet, Channels.DebugChannel);
        }


        //[SerializeField] private PNDevice pl;

        //public Pubnub Pubnub => pl.Connection.PN;

        ////public async void AddSystemChannels()
        ////{
        ////    var response = await Pubnub.SetChannelMetadata()
        ////        .Channel(Channels.Lobby)
        ////        .Name("server-lobby-matches")
        ////        .ExecuteAsync();

        ////    if (response != null)
        ////    {
        ////        bool success = !response.Status.Error;

        ////        Debug.Log($"Success: {success}");

        ////        if(!success)
        ////        {
        ////            Debug.LogWarning($"{response.Status.ErrorData.Information}");
        ////        }
        ////    }
        ////}

        //public async void GetAppChannels()
        //{
        //    var response = await Pubnub.GetAllChannelMetadata()
        //        .IncludeCustom(true)
        //        .ExecuteAsync();

        //    if (response != null)
        //        Debug.Log($"Success: {!response.Status.Error}");

        //    var channels = response.Result.Channels;

        //    foreach (var channel in channels)
        //    {
        //        Debug.Log($"{channel.Name}");
        //    }
        //}
    }
}
