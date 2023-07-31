using NubEval.PubNubWrapper;
using System.Collections.Generic;

namespace NubEval
{
    [System.Serializable]
    public struct MatchStateData : INetworkDataObject
    {
        public MatchStateData(int currentStep, Dictionary<int, ParticipantData> partisipants, Dictionary<int, PlayerState> playerStates, MatchTurnsScript script)
        {
            CurrentScriptStep = currentStep;
            Participants = partisipants;
            PlayerStates = playerStates;
            Script = script;
        }

        //public int Host {get set;};
        public int CurrentScriptStep { get; set; }
        public MatchTurnsScript Script { get; set; }
        public Dictionary<int, ParticipantData> Participants { get; set; }
        public Dictionary<int, PlayerState> PlayerStates { get; set; }
    }
}
