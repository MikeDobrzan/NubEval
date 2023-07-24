using NubEval.Networking;
using NubEval.Networking.PubNubWrapper;
using PubnubApi;
using System.Collections.Generic;
//using UnityEngine;

namespace NubEval
{
    public class NetworkEventsHandler :
        INetworkEventHandler,
        IRemoteLobbyEventsListener
    {
        private readonly PNDevice _pnDevice;
        private readonly UserDeviceData _deviceData;
        private readonly List<ILobbyEventsHandler> _lobbyEventsSubscribers;

        private const string EVENT_JOIN = "join";
        private const string EVENT_LEAVE = "leave";
        private const string EVENT_CHANGE_STATE = "state-change";

        public NetworkEventsHandler(PNDevice pubnub, UserDeviceData device)
        {
            _lobbyEventsSubscribers = new List<ILobbyEventsHandler>();
            _pnDevice = pubnub;
            _deviceData = device;
        }

        void IRemoteLobbyEventsListener.SubscribeLobbyEvents(ILobbyEventsHandler subscriber)
        {
            if (!_lobbyEventsSubscribers.Contains(subscriber))
                _lobbyEventsSubscribers.Add(subscriber);

            _pnDevice.Console.Log($"subs: {_lobbyEventsSubscribers.Count}");
        }

        void INetworkEventHandler.OnPnStatus(Pubnub pn, PNStatus status)
        {
            if (status.Operation == PNOperationType.PNSubscribeOperation)
            {
                if (Channels.Connection.AddressMatch(status.AffectedChannels[0]))
                {
                    _pnDevice.Console.Log($"[Status]: <color=#00FF00> Initial Connection complete</color>");
                    return;
                }
            }

            _pnDevice.Console.Log($"[Status]: {status.Operation}");
        }

        void INetworkEventHandler.OnPnMessage(Pubnub pn, PNMessageResult<object> result)
        {
            _pnDevice.Console.Log($"[MSG]: ch={result.Channel} | {result.Message} | {result.Publisher} | {result.Timetoken}");
        }

        void INetworkEventHandler.OnPnMessageAction(Pubnub pn, PNMessageActionEventResult result)
        {
            _pnDevice.Console.Log($"[MSGAction]: {result.Channel}");
        }

        void INetworkEventHandler.OnPnSignal(Pubnub pn, PNSignalResult<object> result)
        {
            _pnDevice.Console.Log(result.Channel);
        }

        void INetworkEventHandler.OnPnObject(Pubnub pn, PNObjectEventResult result)
        {
            _pnDevice.Console.Log(result.Channel);
        }

        void INetworkEventHandler.OnPnFile(Pubnub pn, PNFileEventResult result)
        {
            _pnDevice.Console.Log(result.Channel);
        }

        async void INetworkEventHandler.OnPnPresence(Pubnub pn, PNPresenceEventResult result)
        {
            _pnDevice.Console.Log($"[Presence] {result.Uuid} | cmd=<{result.Event}> | ch={result.Channel} (Subs:{_lobbyEventsSubscribers.Count})");

            if (result.Channel != null)
            {
                UserId user = result.Uuid;

                if (result.Channel == Channels.DebugChannel.PubNubAddress)
                {

                    UserAccountData userAccountData;

                    var response = await _pnDevice.UserData.GetAccountDataAsync(user);

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

