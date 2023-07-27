using PubnubApi;
using UnityEngine;

namespace NubEval.PubNubWrapper
{
    public class PNDeviceConsole : PNServiceBase
    {
        private readonly UserDeviceData deviceData;

        public PNDeviceConsole(Pubnub pubnub, PNDevice device, UserDeviceData deviceData) : base(pubnub, device)
        {
            this.deviceData = deviceData;
        }

        public void Log(string msg)
        {
            Debug.Log($"[<color=green>{deviceData.DeviceID} </color>]: {msg}");
        }
    }
}
