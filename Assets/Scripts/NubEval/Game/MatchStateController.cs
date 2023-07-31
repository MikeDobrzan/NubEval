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
     
        //TODO: modify state data directly?;
        private readonly  Dictionary<int, PlayerState> _playersStates = new Dictionary<int, PlayerState>();

        public int NextPlayer { get; private set; }
        public MatchStateData CurrentStateData => _matchStateData;

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

            //update state data
            _matchStateData.PlayerStates[new ParticipantID(id)] = newState;
        }

        public void NetworkPublishState(MatchStateData state)
        {
            _matchStateData = state;

            //set next player
            NextPlayer = state.Script.Turns[state.CurrentScriptStep];

            //replace states
            foreach (var key in state.PlayerStates.Keys)
            {
                _playersStates[key.Index] = state.PlayerStates[key];
            }
        }

        public MatchStateData GetStateData()
        {
            return _matchStateData;
        }
    }
}
