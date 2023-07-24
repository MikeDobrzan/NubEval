using NubEval.Networking.PubNubWrapper;
using PubnubApi;
using PubnubApi.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NubEval
{
    public class PNGlobalListener : INetworkEventHandler
    {
        private readonly SubscribeCallbackListener _listener;

        public PNGlobalListener(SubscribeCallbackListener listener)
        {
            _listener = listener;
        }

        void INetworkEventHandler.OnPnFile(Pubnub pn, PNFileEventResult result)
        {
            throw new System.NotImplementedException();
        }

        void INetworkEventHandler.OnPnMessage(Pubnub pn, PNMessageResult<object> result)
        {
            Debug.Log($"NetworkMsg received: ");
        }

        void INetworkEventHandler.OnPnMessageAction(Pubnub pn, PNMessageActionEventResult result)
        {
            throw new System.NotImplementedException();
        }

        void INetworkEventHandler.OnPnObject(Pubnub pn, PNObjectEventResult result)
        {
            throw new System.NotImplementedException();
        }

        void INetworkEventHandler.OnPnPresence(Pubnub pn, PNPresenceEventResult result)
        {
            throw new System.NotImplementedException();
        }

        void INetworkEventHandler.OnPnSignal(Pubnub pn, PNSignalResult<object> result)
        {
            throw new System.NotImplementedException();
        }

        void INetworkEventHandler.OnPnStatus(Pubnub pn, PNStatus result)
        {
            throw new System.NotImplementedException();
        }
    }
}
