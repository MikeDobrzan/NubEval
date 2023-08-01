using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NubEval.PubNubWrapper;
using System.Threading.Tasks;
using PubnubApi;
using System.Threading;
using Newtonsoft.Json;

namespace NubEval.Game.Networking
{
    public class MatchEventsHandler : IMessageEventHandler
    {
        private readonly PNDevice _pnDevice;
        private readonly List<IMatchEventSubscriber> _lobbyEventsSubscribers;

        public MatchEventsHandler(PNDevice device, List<IMatchEventSubscriber> subscribers)
        {
            _pnDevice = device;
            _lobbyEventsSubscribers = subscribers;
        }

        async Task IMessageEventHandler.OnEventAsync(PNMessageResult<object> eventResult, CancellationToken token)
        {
            foreach (var sub in _lobbyEventsSubscribers)
            {               
                await sub.OnMatchStateUpdate((MatchStateData)eventResult.Message);
            }
        }
    }
}
