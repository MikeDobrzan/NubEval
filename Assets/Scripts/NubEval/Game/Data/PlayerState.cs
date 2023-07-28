using NubEval.PubNubWrapper;
using UnityEngine;

namespace NubEval
{
    [System.Serializable]
    public struct PlayerState : INetworkDataObject
    {
        [SerializeField] private bool isKilled;
        [SerializeField] private Vector2 boardPoistion;


        public PlayerState(Vector2 boardPoistion, bool isKilled)
        {
            this.isKilled = isKilled;
            this.boardPoistion = boardPoistion;
        }

        public bool IsKilled { get => isKilled; set => isKilled = value; }
        public Vector2 BoardPoistion { get => boardPoistion; set => boardPoistion = value; }


        public static PlayerState InitialState()
        {
            return new PlayerState(new Vector2(0,0), false);
        }
    }
}
