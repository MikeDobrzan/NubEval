using NubEval.Game.Data;
using NubEval.Game.Networking.Payload;
using PubnubApi;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace NubEval.Game
{
    [System.Serializable]
    public class MatchController : MonoBehaviour
    {
        public int MatchID;
        public MatchConfig Config;
        public MatchRoomStatus MatchStatus;
        public DateTime MatchStart;

        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private PlayerVisual thisPlayerAvatar;
        [SerializeField] private MatchSeed seed;

        private IMatchParticipant localPlayerParticipant;

        public UserId ThisPlayerId;
        public int thisPlayerInMatch;

        private MatchStateController _matchState;

        public Dictionary<int, PlayerState> playerStates;

        [Header("Debug")]
        [SerializeField] MatchStateData debug_matchStateData;

        private async void Start()
        {
            _matchState = new MatchStateController();
            localPlayerParticipant = new LocalPlayerParticipant(playerInput);

            _matchState.NetworkSetState(SetInitialState(seed));

            int nextPlayer = _matchState.NextPlayer;

            await Task.Delay(3000);
            
            if (nextPlayer == thisPlayerInMatch)
            {
                //Wait for the player


                Debug.Log($"Make Your Turn! --> {nextPlayer}");
                var cts = new CancellationTokenSource(10000);

                var playerTurn = PlayerTurn.NewTurn();
                var action = await localPlayerParticipant.RequestActionAsync(playerTurn, cts.Token);

                //Apply the action
                _matchState.ApplyPlayerAction(0, action);// .SetPlayerState(0, newState);

                //Update Avatar
                thisPlayerAvatar.UpdateState(_matchState.GetParticipantState(0));

                Debug.Log("Thank you for Your Turn!");
            }
            else
            {
                Debug.Log($"Not your Turn!  wait for --> {nextPlayer}");
            }
        }


        private MatchStateData SetInitialState(MatchSeed seed)
        {
            Dictionary<int, PlayerState> playerStates = new Dictionary<int, PlayerState>();

            for (int i = 0; i < seed.StartLocations.Count; i++)
            {
                Debug.Log(i);

                var state = new PlayerState(seed.StartLocations[i], false);

                playerStates.Add(i, state);
            }

            return new MatchStateData(0, playerStates, seed.Script);
        }

        private void OnPlayerRequestAction(PlayerState currentState, PlayerAction action)
        {
            if (true)//Check if its current players move
            {
                Vector2 newPos = currentState.BoardPoistion + action.MoveDir;
                var newState = new PlayerState(newPos, false);

                thisPlayerAvatar.UpdateState(newState);
            }
            //else
            //    return;
        }

        private void Dispose()
        {
            thisPlayerAvatar.PlayerActionRequested -= OnPlayerRequestAction;
        }

        private void OnDisable()
        {
            Dispose();
        }
    }
}
