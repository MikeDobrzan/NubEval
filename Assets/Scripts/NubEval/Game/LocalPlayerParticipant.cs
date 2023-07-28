using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace NubEval.Game
{
    public class LocalPlayerParticipant : IMatchParticipant
    {
        private PlayerTurn _playerTurn;
        private PlayerInput _input;
        private PlayerState _state;

        public LocalPlayerParticipant(PlayerInput input)
        {
            _input = input;

            _input.InputDirUpdate += OnInputMove;
            _input.InputPunch += OnPunch;
        }

        private void OnInputMove(PlayerInput.InputMoveDir dir)
        {
            if (_playerTurn.Completed)
                return;

            Debug.Log($"Move: {dir}");

            _playerTurn.PlayerAction = MakePlayerAction(dir);
            _playerTurn.Complete();
        }

        private void OnPunch()
        {
            if (_playerTurn.Completed)
                return;

            Debug.Log($"Punch!");
        }

        private PlayerAction MakePlayerAction(PlayerInput.InputMoveDir dir)
        {
            Vector2 newPos = default;

            switch (dir)
            {
                case PlayerInput.InputMoveDir.up:
                    newPos = new Vector2(0, 1);
                    break;
                case PlayerInput.InputMoveDir.down:
                    newPos = new Vector2(0, -1);
                    break;
                case PlayerInput.InputMoveDir.left:
                    newPos = new Vector2(-1, 0);
                    break;
                case PlayerInput.InputMoveDir.right:
                    newPos = new Vector2(1, 0);
                    break;
                case PlayerInput.InputMoveDir.stay:
                    newPos = new Vector2(0, 0);
                    break;
                default:
                    break;
            }
            return new PlayerAction(newPos, false);
        }


        async Task<PlayerAction> IMatchParticipant.RequestActionAsync(PlayerTurn turn, CancellationToken token)
        {
            _playerTurn = turn;
            while (!_playerTurn.Completed && !token.IsCancellationRequested)
            {
                await Task.Yield();
            }

            return _playerTurn.PlayerAction;
        }
    }
}
