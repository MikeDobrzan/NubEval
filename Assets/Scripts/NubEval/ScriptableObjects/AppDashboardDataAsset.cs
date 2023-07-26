using NubEval.Networking.Payloads;
using UnityEngine;

namespace NubEval
{
    [CreateAssetMenu(fileName = "appDashboadData-", menuName = "Debug/Mocks/App Dashboard")]
    public class AppDashboardDataAsset : ScriptableObject
    {
        [SerializeField] private MatchAnnouncement matchAnnouncemet;

        public MatchAnnouncement MatchAnnouncemet { get => matchAnnouncemet; }
    }
}
