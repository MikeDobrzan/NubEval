using NubEval.Networking.PubNubWrapper;
using PubnubApi;
using UnityEngine;

namespace NubEval
{
    public class NetworkEventsHandler : INetworkEventHandler
    {
        void INetworkEventHandler.OnPnStatus(Pubnub pn, PNStatus status)
        {
            string msg = status.Category == PNStatusCategory.PNConnectedCategory ? "Connected" : "Not connected";

            Debug.Log(msg);
        }

        void INetworkEventHandler.OnPnMessage(Pubnub pn, PNMessageResult<object> result)
        {
            Debug.Log($"Message received: ch={result.Channel} | {result.Message} | {result.Publisher} | {result.Timetoken}");
        }

        void INetworkEventHandler.OnPnMessageAction(Pubnub pn, PNMessageActionEventResult result)
        {
            Debug.Log(result.Channel);
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

        void INetworkEventHandler.OnPnPresence(Pubnub pn, PNPresenceEventResult result)
        {
            Debug.Log(result.Event);
        }
    }
}
