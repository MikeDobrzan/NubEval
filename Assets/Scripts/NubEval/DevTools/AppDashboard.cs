using NubEval.Game;
using NubEval.Game.Networking;
using NubEval.Game.Networking.Payload;
using NubEval.PubNubWrapper;
using System.Threading.Tasks;
using UnityEngine;

namespace NubEval.DevTools
{
    public class AppDashboard : MonoBehaviour
    {
        [SerializeField] private AppDashboardDataAsset mockData;
        [SerializeField] private Bootloader bootloader;

        private PNDevice _mainDevice;

        private void Awake()
        {
            bootloader.OnPubNubDeviceInitialized += Construct;
            if (_mainDevice == null)
            {
                _mainDevice = bootloader.PNDevice;
            }
        }

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
            await _mainDevice.Presence.SetPresenceState(Channels.Lobby, mockData.States);
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
            var announcement = mockData.MatchAnnouncement;

            //create match channel (with presence)
            Channel matchChannel = Channels.GetMatchChannel(mockData.MatchAnnouncement.MatchConfig.MatchID);

            var announcementResponse = await _mainDevice.MessageDispatcher.SendMsg(mockData.MatchAnnouncement, Channels.MatchesAnnouncements);

            //get timestamped id from the message publishing
            announcement.AnnouncementMessage = announcementResponse.Item2;

            await _mainDevice.Subscriptions.SubscribeChannels(matchChannel);
            await _mainDevice.MetadataChannels.SetDefaultCustomData(matchChannel, announcement);

            await Task.Delay(2000); //give it some time and broadcast announcement //to debounce announcements




            var matchData = await _mainDevice.MetadataChannels.GetDefaultCustomData<MatchRoomAnnouncement>(matchChannel);
            Debug.Log($"AnnouncementID=({matchData.AnnouncementMessage.Channel},{matchData.AnnouncementMessage.Timetoken}) : {matchData.MatchConfig.Name}");

            await Task.Delay(4000);


            //delete announcemet for test

            await _mainDevice.MessageDispatcher.DeleteFromHistory(matchData.AnnouncementMessage);
        }

        public async void OnBtnDeletematchroom()
        {

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
