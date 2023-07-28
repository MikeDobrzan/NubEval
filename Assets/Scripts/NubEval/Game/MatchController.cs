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

        private IMatchParticipant localPlayerParticipant;

        public UserId ThisPlayerId;
        public int thisPlayerInMatch;

        private MatchState _matchState;

        public Dictionary<int, PlayerState> playerStates;

        private async void Start()
        {
            _matchState = new MatchState();
            _matchState.ResetToInitialState(1);

            localPlayerParticipant = new LocalPlayerParticipant(playerInput);

            await Task.Delay(3000);
            Debug.Log("Make Your Turn!");
            var cts = new CancellationTokenSource(10000);

            var playerTurn = PlayerTurn.NewTurn();
            var action = await localPlayerParticipant.RequestActionAsync(playerTurn, cts.Token);

            //Apply the action
            _matchState.ApplyPlayerAction(0, action);// .SetPlayerState(0, newState);

            //Update Avatar
            thisPlayerAvatar.UpdateState(_matchState.GetParticipantState(0));

            Debug.Log("Thank you for Your Turn!");
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
