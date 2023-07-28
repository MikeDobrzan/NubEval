using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NubEval
{
    public struct MatchState
    {
        public List<PlayerState> playersStates;
        public int nextInTurn;
        public Dictionary<int, int> KillablePlayers;
    }
}
