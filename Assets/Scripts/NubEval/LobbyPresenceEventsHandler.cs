using PubnubApi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Threading;
using NubEval.Networking.PubNubWrapper;

namespace NubEval
{
    public class LobbyPresenceEventsHandler : IPresenceEventHandler
    {
        private PNDevice _pnDevice;
        private readonly List<ILobbyEventsSubscriber> _lobbyEventsSubscribers;

        private readonly Channel _channel;

        public LobbyPresenceEventsHandler(PNDevice device, List<ILobbyEventsSubscriber> lobbyEventsSubscribers)
        {
            _pnDevice = device;
            _lobbyEventsSubscribers = lobbyEventsSubscribers;
            _channel = Channels.Lobby;
        }

        async Task IPresenceEventHandler.OnEventAsync(PNPresenceEventResult eventResult, CancellationToken token)
        {
            Debug.Log($"handle lobby Event uuid={eventResult.Uuid}");

            try
            {
                token.ThrowIfCancellationRequested();

                UserId user = eventResult.Uuid;
                UserAccountData userAccountData;

                var states = await _pnDevice.Presence.GetStates(_channel, user);
                var acountDataResponse = await _pnDevice.MetadataUsers.GetAccountDataAsync(user);

                if (acountDataResponse.Item1)
                    userAccountData = acountDataResponse.Item2;
                else
                    userAccountData = new UserAccountData(user);

                var eventType = ResponseNormalization.ToPresenceEventType(eventResult.Event);
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
                    case PresencelEvent.ChangeState: //CHANGE STATE
                        foreach (var sub in _lobbyEventsSubscribers)
                        {
                            sub.OnUserChangeState(user, userAccountData, states);
                        }
                        break;
                    case PresencelEvent.TimedOut: //TIMED OUT
                        foreach (var sub in _lobbyEventsSubscribers)
                        {
                            sub.OnUserLeave(user, userAccountData);
                        }
                        break;
                    default:
                        break;
                }
            }
            catch
            {
                Debug.LogWarning($"[LobbyPresenceEventsHandler] timed out or failed.");
            }
        }
    }
}
