using NubEval.PubNubWrapper;
using System.Collections.Generic;

namespace NubEval
{
    [System.Serializable]
    public struct MatchStateData : INetworkDataObject
    {      
        public MatchStateData(int currentStep, Dictionary<ParticipantID, PlayerState> playerStates, MatchTurnsScript script)
        {
            CurrentScriptStep = currentStep;
            PlayerStates = playerStates;
            Script = script;
        }

        //public int Host {get set;};
        public int CurrentScriptStep { get; set; }
        public MatchTurnsScript Script { get; set; }        
        public Dictionary<ParticipantID, PlayerState> PlayerStates { get; set; }
    }
}
