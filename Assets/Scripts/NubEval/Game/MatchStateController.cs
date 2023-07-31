using Newtonsoft.Json;
using PubnubApi;
using System.Collections.Generic;
using System.Linq;
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
        private readonly Dictionary<int, PlayerState> _playersStates = new Dictionary<int, PlayerState>();

        public int NextPlayerIndex { get; private set; }

        public string NextPlayerID
        {
            get
            {
                if (_matchStateData.Participants.TryGetValue(NextPlayerIndex, out ParticipantData data))
                    return data.PnUser;
                else
                {
                    Debug.Log("MatchStateData corrupted");
                    return default;
                }
            }
        }

        public MatchStateData CurrentStateData => _matchStateData;
        public string MatchStateJSON
        {
            get
            {
                return JsonConvert.SerializeObject(_matchStateData);
            }
        }

        public MatchStateController()
        {
            _playersStates = new Dictionary<int, PlayerState>();
        }

        public PlayerState GetParticipantState(int id)
        {
            return _playersStates[id];
        }

        public int GetPlayerIndex(UserId user)
        {
            var record = _matchStateData.Participants.FirstOrDefault(u => u.Value.PnUser == user);
            return record.Value.Index;
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

            _matchStateData.CurrentScriptStep += 1;

            //update state data
            _matchStateData.PlayerStates[id] = newState;
        }

        public void NetworkPublishState(MatchStateData state)
        {
            _matchStateData = state;

            Debug.Log(MatchStateJSON);

            //set next player
            NextPlayerIndex = state.Script.Turns[state.CurrentScriptStep];

            //replace states
            foreach (var key in state.PlayerStates.Keys)
            {
                _playersStates[key] = state.PlayerStates[key];
            }
        }

        public MatchStateData GetStateData()
        {
            return _matchStateData;
        }
    }
}
