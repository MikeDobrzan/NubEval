using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NubEval.Game.Networking.Data
{
    [System.Serializable]
    public struct MessageID
    {
        public string Channel;
        public long Timetoken;

        public MessageID(string channel, long timetoken)
        {
            Channel = channel;
            Timetoken = timetoken;
        }
    }
}
