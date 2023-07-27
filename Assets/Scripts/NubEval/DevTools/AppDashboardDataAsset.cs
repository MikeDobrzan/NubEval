using NubEval.Game.Networking;
using NubEval.Game.Networking.Payload;
using System.Collections.Generic;
using UnityEngine;

namespace NubEval.DevTools
{
    [CreateAssetMenu(fileName = "appDashboadData-", menuName = "Debug/Mocks/App Dashboard")]
    public class AppDashboardDataAsset : ScriptableObject
    {
        [SerializeField] private MatchRoomAnnouncement matchAnnouncemet;
        [SerializeField] private List<PresenceState> states;

        public MatchRoomAnnouncement MatchAnnouncemet { get => matchAnnouncemet; }
        public List<PresenceState> States { get => states; }
    }
}
