using PubnubApi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NubEval
{
    public class PNServiceBase
    {
        private readonly Pubnub _pn;

        protected UserId CurrentUserId => _pn.GetCurrentUserId();
        protected Pubnub Pubnub => _pn;

        public PNServiceBase(Pubnub pubnub)
        {
            _pn = pubnub;
        }
    }
}
