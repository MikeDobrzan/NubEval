using NubEval.Networking.Payloads;
using System.Collections.Generic;
using UnityEngine;

namespace NubEval
{
    [CreateAssetMenu(fileName = "appDashboadData-", menuName = "Debug/Mocks/App Dashboard")]
    public class AppDashboardDataAsset : ScriptableObject
    {
        [SerializeField] private MatchAnnouncement matchAnnouncemet;
        [SerializeField] private List<PresenceState> states;

        public MatchAnnouncement MatchAnnouncemet { get => matchAnnouncemet; }
        public List<PresenceState> States { get => states; }
    }
}
