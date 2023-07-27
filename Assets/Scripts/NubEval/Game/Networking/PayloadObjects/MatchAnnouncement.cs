using NubEval.PubNubWrapper;
using UnityEngine;

namespace NubEval.Game.Networking.Payload
{
    [System.Serializable]
    public struct MatchAnnouncement : INetworkDataObject
    {
        [SerializeField] private MatchConfig config;
        [SerializeField] private MatchStatus status;

        public MatchConfig MatchConfig { get => config; set => config = value; }
        public MatchStatus MatchStatus { get => status; set => status = value; }
        //public AnnouncementStatus AnnouncementStatus { get => status; set => status = value; }
    }
}
