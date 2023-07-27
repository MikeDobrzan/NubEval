using NubEval.PubNubWrapper;
using PubnubApi;
using System.Collections.Generic;

using System.Threading;

namespace NubEval.Game.Networking
{
    public class NetworkEventsHandler :
        INetworkEventHandler,
        IRemoteLobbyEventsListener
    {
        private readonly PNDevice _pnDevice;
        private readonly UserDeviceData _deviceData;
        private readonly List<ILobbyEventsSubscriber> _lobbyEventsSubscribers;
        private readonly Pubnub _pnApi;

        private readonly IPresenceEventHandler _handlerLobbyPresence;
        private readonly IPresenceEventHandler _handlerDebugPresence;

        public NetworkEventsHandler(Pubnub api, PNDevice pubnub, UserDeviceData device)
        {
            _lobbyEventsSubscribers = new List<ILobbyEventsSubscriber>();
            _pnDevice = pubnub;
            _deviceData = device;
            _pnApi = api;

            _handlerLobbyPresence = new LobbyPresenceEventsHandler(pubnub, _lobbyEventsSubscribers);
        }

        void IRemoteLobbyEventsListener.SubscribeToLobbyEvents(ILobbyEventsSubscriber subscriber)
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
                    channels += $"{ch}; ";

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
            if (result == null || result.Channel == null)
                return;

            if (result.Channel == Channels.Lobby.PubNubAddress)
            {
                var cts = new CancellationTokenSource(3000);
                await _handlerLobbyPresence.OnEventAsync(result, cts.Token);
            }

            //if (result.Channel == Channels.DebugChannel.PubNubAddress)
            //{
            //    var cts = new CancellationTokenSource(3000);
            //    await _handlerLobbyPresence.OnEventAsync(result, cts.Token);
            //}
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

