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

        private const string EVENT_JOIN = "join";
        private const string EVENT_LEAVE = "leave";
        private const string EVENT_CHANGE_STATE = "state-change";

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

            Debug.Log($"{_deviceData} subs: {_lobbyEventsSubscribers.Count}");
        }

        void INetworkEventHandler.OnPnStatus(Pubnub pn, PNStatus status)
        {
            if (status.Operation == PNOperationType.PNSubscribeOperation)
            {
                if (Channels.Connection.AddressMatch(status.AffectedChannels[0]))
                {
                    Debug.Log($"{_deviceData}[Status]<color=#00FF00> Initial Connection complete</color>");
                    return;
                }
            }

            Debug.Log($"{_deviceData}[{status.Operation}]");
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
            //Debug.Log($"{_deviceData}[Presence] {result.Uuid} <{result.Event}> ch={result.Channel} (Subs:{_lobbyEventsSubscribers.Count})");

            if (result.Channel != null)
            {
                UserId user = result.Uuid;

                if (result.Channel == Channels.DebugChannel.PubNubAddress)
                {

                    UserAccountData userAccountData;

                    var response = await _pubnub.UserData.GetAccountDataAsync(user);

                    if (response.Item1)
                    {
                        userAccountData = response.Item2;

                        if (string.Equals(result.Event, EVENT_JOIN))
                        {
                            foreach (var sub in _lobbyEventsSubscribers)
                            {
                                sub.OnUserJoin(user, userAccountData);
                            }
                        }

                        if (string.Equals(result.Event, EVENT_LEAVE))
                        {
                            foreach (var sub in _lobbyEventsSubscribers)
                            {
                                sub.OnUserLeave(user, userAccountData);
                            }
                        }
                    }
                }
            }
        }
    }
}

