using NubEval.Game.Networking.Payload;
using System;

namespace NubEval.Game
{
    [System.Serializable]
    public class Match
    {
        public int MatchID;
        public MatchConfig Config;
        public MatchStatus MatchStatus;
        public DateTime MatchStart;
    }
}
