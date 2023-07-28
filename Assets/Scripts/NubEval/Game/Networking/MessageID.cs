using Newtonsoft.Json;
using NubEval.PubNubWrapper;
using UnityEngine;

namespace NubEval.Game.Networking
{
    [System.Serializable]
    public struct MessageID : INetworkDataObject
    {
        [SerializeField] private string channel;
        [SerializeField] private long timetoken;

        public string Channel { get => channel; set => channel = value; }
        public long Timetoken { get => timetoken; set => timetoken = value; }

        [JsonIgnore] public long DelStart => Timetoken - 1;
        [JsonIgnore] public long DelEnd => Timetoken;

        public MessageID(string channel, long timetoken)
        {
            this.channel = channel;
            this.timetoken = timetoken;
        }
    }
}
