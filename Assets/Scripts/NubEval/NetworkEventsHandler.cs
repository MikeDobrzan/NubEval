using NubEval.Networking;
using NubEval.Networking.PubNubWrapper;
using PubnubApi;
using System.Collections.Generic;
using UnityEngine;

namespace NubEval
{
    public class NetworkEventsHandler :
        INetworkEventHandler,
        IRemoteLobbyEventsListener
    {
        private readonly PNDevice _pubnub;
        private readonly UserDeviceData _deviceData;
        private readonly List<ILobbyEventsHandler> _lobbyEventsSubscribers;

        public NetworkEventsHandler(PNDevice pubnub, UserDeviceData device)
        {
            _lobbyEventsSubscribers = new List<ILobbyEventsHandler>();
            _pubnub = pubnub;
            _deviceData = device;
        }

        void IRemoteLobbyEventsListener.SubscribeLobbyEvents(ILobbyEventsHandler subscriber)
        {
            if (!_lobbyEventsSubscribers.Contains(subscriber))
                _lobbyEventsSubscribers.Add(subscriber);

            Debug.LogWarning($"{_deviceData} subs: {_lobbyEventsSubscribers.Count}");
        }

        void INetworkEventHandler.OnPnStatus(Pubnub pn, PNStatus status)
        {
            string msg = status.Category == PNStatusCategory.PNConnectedCategory ? "Connected" : "Not connected";

            Debug.Log($"{_deviceData}[Status] {msg}");
        }

        void INetworkEventHandler.OnPnMessage(Pubnub pn, PNMessageResult<object> result)
        {
            Debug.Log($"{_deviceData}[MSG] ch={result.Channel} | {result.Message} | {result.Publisher} | {result.Timetoken}");
        }

        void INetworkEventHandler.OnPnMessageAction(Pubnub pn, PNMessageActionEventResult result)
        {
            Debug.Log($"{_deviceData}[MSGAction] {result.Channel}");
        }

        void INetworkEventHandler.OnPnSignal(Pubnub pn, PNSignalResult<object> result)
        {
            Debug.Log(result.Channel);
        }

        void INetworkEventHandler.OnPnObject(Pubnub pn, PNObjectEventResult result)
        {
            Debug.Log(result.Channel);
        }

        void INetworkEventHandler.OnPnFile(Pubnub pn, PNFileEventResult result)
        {
            Debug.Log(result.Channel);
        }

        async void INetworkEventHandler.OnPnPresence(Pubnub pn, PNPresenceEventResult result)
        {
            Debug.Log($"{_deviceData}[Presence] {result.Uuid} <{result.Event}> ch={result.Channel} (Subs:{_lobbyEventsSubscribers.Count})");

            if (result.Channel != null)
            {
                if (result.Channel == Channels.MainChannel.PubNubAddress)
                {
                    UserId user = result.Uuid;
                    UserAccountData userAccountData;

                    var response = await _pubnub.UserData.GetAccountDataAsync(user);

                    if (response.Item1)
                    {
                        userAccountData = response.Item2;

                        foreach (var sub in _lobbyEventsSubscribers)
                        {
                            sub.OnUserJoined(user, userAccountData);
                        }
                    }
                }
            }
        }
    }
}
