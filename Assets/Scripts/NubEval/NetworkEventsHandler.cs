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
        private readonly PNDevice _pnDevice;
        private readonly UserDeviceData _deviceData;
        private readonly List<ILobbyEventsHandler> _lobbyEventsSubscribers;
        private readonly Pubnub _pnApi;



        public NetworkEventsHandler(Pubnub api, PNDevice pubnub, UserDeviceData device)
        {
            _lobbyEventsSubscribers = new List<ILobbyEventsHandler>();
            _pnDevice = pubnub;
            _deviceData = device;
            _pnApi = api;
        }

        void IRemoteLobbyEventsListener.SubscribeToLobbyEvents(ILobbyEventsHandler subscriber)
        {
            if (!_lobbyEventsSubscribers.Contains(subscriber))
                _lobbyEventsSubscribers.Add(subscriber);

            _pnDevice.Console.Log($"subs: {_lobbyEventsSubscribers.Count}");
        }

        void INetworkEventHandler.OnPnStatus(Pubnub pn, PNStatus status)
        {
            string channels = $"ch=";

            if (FindListenerTarget(pn))
                return;

            if (status.Operation == PNOperationType.PNSubscribeOperation)
            {
                if (Channels.Connection.AddressMatch(status.AffectedChannels[0]))
                {
                    _pnDevice.Console.Log($"[Status]: <color=#00FF00> Initial Connection complete</color>");
                    return;
                }
            }

            if (status.Operation == PNOperationType.PNSubscribeOperation || status.Operation == PNOperationType.PNUnsubscribeOperation)
            {
                foreach (var ch in status.AffectedChannels)               
                    channels += ch;               

                status.AffectedChannels.ToString();
            }

            _pnDevice.Console.Log($"[Status]: {status.Operation} | {channels}");
        }

        void INetworkEventHandler.OnPnMessage(Pubnub pn, PNMessageResult<object> result)
        {
            if (FindListenerTarget(pn))
                return;

            _pnDevice.Console.Log($"[MSG]: ch={result.Channel} | {result.Message} | {result.Publisher} | {result.Timetoken}");
        }

        void INetworkEventHandler.OnPnMessageAction(Pubnub pn, PNMessageActionEventResult result)
        {
            if (FindListenerTarget(pn))
                return;

            _pnDevice.Console.Log($"[MSGAction]: {result.Channel}");
        }

        void INetworkEventHandler.OnPnSignal(Pubnub pn, PNSignalResult<object> result)
        {
            if (FindListenerTarget(pn))
                return;

            _pnDevice.Console.Log(result.Channel);
        }

        void INetworkEventHandler.OnPnObject(Pubnub pn, PNObjectEventResult result)
        {
            if (FindListenerTarget(pn))
                return;

            _pnDevice.Console.Log(result.Channel);
        }

        void INetworkEventHandler.OnPnFile(Pubnub pn, PNFileEventResult result)
        {
            if (FindListenerTarget(pn))
                return;

            _pnDevice.Console.Log(result.Channel);
        }

        async void INetworkEventHandler.OnPnPresence(Pubnub pn, PNPresenceEventResult result)
        {
            _pnDevice.Console.Log($"[Presence] {result.Uuid} | cmd=<{result.Event}> | ch={result.Channel} (Subs:{_lobbyEventsSubscribers.Count})");

            //validate response
            if (result.Channel == null)
                return;

            if (result.Channel != Channels.DebugChannel.PubNubAddress) //only listen to the debug channel for now
                return;

            UserId user = result.Uuid;

            if (!ResponseNormalization.IsValidPresenceState(result.State))
                Debug.LogWarning($"States are corrupted: {result.State == null}"); //but it will try to compile usefull data

            var states = ResponseNormalization.ToPresenceStates(result.State); //if the dict is corrupted this simply returns empty list


            UserAccountData userAccountData;

            var acountDataResponse = await _pnDevice.UserData.GetAccountDataAsync(user);

            if (acountDataResponse.Item1)
            {
                userAccountData = acountDataResponse.Item2;

                var eventType = ResponseNormalization.ToPresenceEventType(result.Event);




                switch (eventType)
                {
                    case PresencelEvent.unknown:
                        Debug.LogWarning("Unknown user state received!");
                        break;
                    case PresencelEvent.Join: // JOIN
                        foreach (var sub in _lobbyEventsSubscribers)
                        {
                            sub.OnUserJoin(user, userAccountData, states);
                        }
                        break;
                    case PresencelEvent.Leave: // LEAVE

                        foreach (var sub in _lobbyEventsSubscribers)
                        {
                            sub.OnUserLeave(user, userAccountData);
                        }

                        break;
                    case PresencelEvent.ChangeState:
                        break;
                    case PresencelEvent.TimedOut:
                        break;
                    default:
                        break;
                }
            }
        }

        private bool FindListenerTarget(Pubnub pn)
        {
            //Debug.LogWarning($"incomingID={pn.GetCurrentUserId()} | listenerID={_pnApi.GetCurrentUserId()}");

            if (pn != _pnApi)
                return true;

            return false;
        }
    }
}

