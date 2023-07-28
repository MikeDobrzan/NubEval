using NubEval.PubNubWrapper;
using UnityEngine;

namespace NubEval
{
    [System.Serializable]
    public struct PlayerState : INetworkDataObject
    {
        [SerializeField] private bool isKilled;
        [SerializeField] private Vector2 boardPoistion;


        public PlayerState(bool isKilled, Vector2 boardPoistion, Color color)
        {
            this.isKilled = isKilled;
            this.boardPoistion = boardPoistion;
        }

        public bool IsKilled { get => isKilled; set => isKilled = value; }
        public Vector2 BoardPoistion { get => boardPoistion; set => boardPoistion = value; }

    }
}
