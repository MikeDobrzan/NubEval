using PubnubApi;

namespace NubEval.Networking.PubNubWrapper
{
    /// <summary>
    /// PubNub network events callbacks
    /// </summary>
    public interface INetworkEventHandler
    {
        void OnPnStatus(Pubnub pn, PNStatus result);
        void OnPnMessage(Pubnub pn, PNMessageResult<object> result);
        void OnPnMessageAction(Pubnub pn, PNMessageActionEventResult result);
        void OnPnSignal(Pubnub pn, PNSignalResult<object> result);
        void OnPnObject(Pubnub pn, PNObjectEventResult result);
        void OnPnFile(Pubnub pn, PNFileEventResult result);
        void OnPnPresence(Pubnub pn, PNPresenceEventResult result);        
    }
}
