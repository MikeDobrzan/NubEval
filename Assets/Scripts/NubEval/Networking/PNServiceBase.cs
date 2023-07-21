using PubnubApi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NubEval
{
    public class PNServiceBase
    {
        private readonly Pubnub _pn;
        private readonly PNDevice _device;

        protected Pubnub PNApi => _pn;
        protected PNDevice PNDevice => _device;

        public PNServiceBase(Pubnub pubnub, PNDevice device)
        {
            _pn = pubnub;  
            _device = device;
        }
    }
}
