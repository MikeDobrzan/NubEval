using System.Collections.Generic;
using UnityEngine;

namespace NubEval
{
    /// <summary>
    /// Initial Match State
    /// </summary>
    [System.Serializable]
    public struct MatchSeed
    {
        public MatchTurnsScript Script;
        public List<Vector2> StartLocations;
    }
}
