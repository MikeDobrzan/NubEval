using NubEval.PubNubWrapper;
using System.Collections.Generic;
using UnityEngine;

namespace NubEval
{
    [System.Serializable]
    public struct MatchTurnsScript : INetworkDataObject
    {
        [SerializeField] private List<int> turns;
        public List<int> Turns { get => turns; set => turns = value; }
    }
}
