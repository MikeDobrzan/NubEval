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

        public MatchConfig(int matchID, MatchType type, string name)
        {
            this.matchID = matchID;
            this.type = type;
            this.name = name;
        }

        public int MatchID { get => matchID; set => matchID = value; }
        public MatchType MatchType { get => type; set => type = value; }
        public string Name { get => name; set => name = value; }

        [JsonIgnore]
        public Channel Channel { get => Channels.GetMatchChannel(matchID); }
    }

    public enum MatchType
    {
        Normal = 1,
        Ranked = 2,
    }
}
