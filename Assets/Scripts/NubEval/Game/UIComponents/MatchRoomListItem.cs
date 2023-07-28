using PubnubApi;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

namespace NubEval.Game.UIComponents
{
    public class MatchRoomListItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI matchNameText;
        [SerializeField] private TextMeshProUGUI participantsCountText;
        [SerializeField] private TextMeshProUGUI matchStatus;
        [SerializeField] private Button btnJoin;

        public void UpdateData(MatchRoomListItemData data)
        {
            matchNameText.text = data.MatchName;
            participantsCountText.text = $"({data.JoinedParticipants}/{data.MaxParticipants})";
            matchStatus.text = $"[{data.MatchStatus}]";
        }
    }

    public struct MatchRoomListItemData
    {
        public MatchRoomListItemData(int matchID, string matchName, int joinedParticipants, string maxParticipants, string matchStatus, bool joinable)
        {
            MatchID = matchID;
            MatchName = matchName;
            JoinedParticipants = joinedParticipants;
            MaxParticipants = maxParticipants;
            MatchStatus = matchStatus;
            Joinable = joinable;
        }

        public int MatchID { get; }
        public string MatchName { get; }
        public int JoinedParticipants { get; }
        public string MaxParticipants { get; }
        public string MatchStatus { get; }
        public bool Joinable { get; }

    }
}
