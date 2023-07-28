using NubEval.Game.Data;
using NubEval.PubNubWrapper;
using UnityEngine;

namespace NubEval.Game.Networking.Payload
{
    [System.Serializable]
    public struct MatchRoomAnnouncement : INetworkDataObject
    {
        [SerializeField] private MessageID announcementID;
        [SerializeField] private MatchConfig config;
        [SerializeField] private MatchRoomStatus status;

        public MessageID AnnouncementMessage { get => announcementID; set => announcementID = value; }
        public MatchConfig MatchConfig { get => config; set => config = value; }
        public MatchRoomStatus MatchStatus { get => status; set => status = value; }
    }
}