using Newtonsoft.Json;
using NubEval.Networking;
using UnityEngine;

namespace NubEval
{
    [System.Serializable]
    public struct MatchConfig : INetworkDataObject
    {
        [SerializeField] private int matchID;
        [SerializeField] private MatchType type;
        [SerializeField] private string name;

        public int MatchID { get => matchID; }
        public MatchType MatchType { get => type; }
        public string Name { get => name; }

        [JsonIgnore]
        public Channel Channel { get => Channels.GetMatchChannel(matchID); }
    }

    public enum MatchType
    {
        Normal = 1,
        Ranked = 2,
    }
}
