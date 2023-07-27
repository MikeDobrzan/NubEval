using NubEval.Game.Networking.Payload;
using NubEval.Game.Data;
using System;

namespace NubEval.Game
{
    [System.Serializable]
    public class MatchController
    {
        public int MatchID;
        public MatchConfig Config;
        public MatchRoomStatus MatchStatus;
        public DateTime MatchStart;
    }
}
