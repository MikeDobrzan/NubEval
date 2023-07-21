using PubnubApi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NubEval
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
