using NubEval.Networking.PubNubWrapper;
using PubnubApi;
using UnityEngine;

namespace NubEval
{
    public class NetworkEventsHandler : INetworkEventHandler
    {
        private readonly UserDeviceData _deviceData;

        public NetworkEventsHandler(UserDeviceData device)
        {
            _deviceData = device;
        }

        void INetworkEventHandler.OnPnStatus(Pubnub pn, PNStatus status)
        {
            string msg =  status.Category == PNStatusCategory.PNConnectedCategory ? "Connected" : "Not connected";

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

        void INetworkEventHandler.OnPnPresence(Pubnub pn, PNPresenceEventResult result)
        {
            Debug.Log($"{_deviceData}[Presence] {result.Uuid} <{result.Event}> ch={result.Channel}");
        }
    }
}
