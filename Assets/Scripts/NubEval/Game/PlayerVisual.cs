using System;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace NubEval.Game
{
    public class PlayerVisual : MonoBehaviour
    {

        [SerializeField] private PlayerInput _input;

        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private SpriteRenderer mainSprite;
        [SerializeField] private SpriteRenderer deadSprite;

        [Header("Debug")]
        [SerializeField] private PlayerState playerState;

        public event Action<PlayerState, PlayerAction> PlayerActionRequested;

        [SerializeField] public bool _turnEnded = false;


        public void AttachInput(PlayerInput input)
        {
            _input = input;

            _input.InputDirUpdate += OnInputMove;
            _input.InputPunch += OnPunch;
        }


        // Start is called before the first frame update
        private void SetState(PlayerState state)
        {
            gameObject.transform.position = state.BoardPoistion;
            deadSprite.enabled = state.IsKilled;
            playerState = state;
        }

        private void SetVisual(PlayerData data)
        {
            nameText.text = data.Name;
            mainSprite.color = data.Color;
        }

        public void UpdateData(PlayerData data, bool local = false)
        {
            SetVisual(data);
            SetAsLocalPlayer(local, data);               
        }

        public void UpdateState(PlayerState state)
        {
            SetState(state);
        }

        public void SetAsLocalPlayer(bool local, PlayerData data)
        {
            nameText.color = local ? Color.green : Color.white;
            nameText.text = local ? data.Name + " (You)" : data.Name;
        }

        private void OnInputMove(PlayerInput.InputMoveDir dir)
        {
            if (_turnEnded)
                return;

            Debug.Log($"Move: {dir}");
            PlayerActionRequested?.Invoke(playerState, MakePlayerAction(dir));
        }

        private void OnPunch()
        {
            if (_turnEnded)
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

            _turnEnded = true;

            return new PlayerAction(newPos, false);
        }
    }
}
