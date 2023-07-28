using System.Collections.Generic;
using UnityEngine;

namespace NubEval
{
    /// <summary>
    /// match state controller
    /// </summary>
    public class MatchState
    {
        public int nextInTurn;
        private readonly  Dictionary<int, PlayerState> _playersStates = new Dictionary<int, PlayerState>();

        public MatchState()
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
            Vector2 newPos = GetParticipantState(0).BoardPoistion + action.MoveDir;
            var newState = new PlayerState(newPos, false);

            //Set the new state 
            SetPlayerState(0, newState);
        }

        public void ResetToInitialState(int participantCount)
        {
            for (int i = 0; i < participantCount; i++)
            {
                _playersStates.Add(i, PlayerState.InitialState());
            }
        }
    }
}
