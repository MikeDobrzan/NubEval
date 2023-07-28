using System.Collections.Generic;
using UnityEngine;

namespace NubEval
{
    /// <summary>
    /// match state controller
    /// </summary>
    public class MatchStateController
    {
        private MatchStateData _matchStateData;
     
        private readonly  Dictionary<int, PlayerState> _playersStates = new Dictionary<int, PlayerState>();

        public int NextPlayer { get; private set; }

        public MatchStateController()
        {
            _playersStates = new Dictionary<int, PlayerState>();
        }

        public PlayerState GetParticipantState(int id)
        {
            return _playersStates[id];
        }

        private void SetPlayerState(int id, PlayerState state)
        {
            _playersStates[id] = state;
        }

        public void ApplyPlayerAction(int id, PlayerAction action)
        {
            //Crteate new state
            Vector2 newPos = GetParticipantState(id).BoardPoistion + action.MoveDir;
            var newState = new PlayerState(newPos, false);

            //Set the new state 
            SetPlayerState(id, newState);

            //---------------------------------> !!!!!!! TODO: update state data
        }

        public void NetworkSetState(MatchStateData state)
        {
            _matchStateData = state;

            //set next player
            NextPlayer = state.Script.Turns[state.CurrentScriptStep];

            //replace states
            foreach (var record in state.PlayerStates)
            {
                _playersStates[record.Key] = state.PlayerStates[record.Key];
            }
        }

        public MatchStateData GetStateData()
        {
            return _matchStateData;
        }
    }
}
