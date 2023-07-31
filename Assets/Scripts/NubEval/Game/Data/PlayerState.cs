using NubEval.PubNubWrapper;
using UnityEngine;

namespace NubEval
{
    [System.Serializable]
    public struct PlayerState : INetworkDataObject
    {
        [SerializeField] private bool isKilled;
        [SerializeField] private float2 boardPoistion;


        public PlayerState(float2 boardPoistion, bool isKilled)
        {
            this.isKilled = isKilled;
            this.boardPoistion = boardPoistion;
        }

        public bool IsKilled { get => isKilled; set => isKilled = value; }
        public float2 BoardPoistion { get => boardPoistion; set => boardPoistion = value; }


        public static PlayerState InitialState()
        {
            return new PlayerState(new float2(0,0), false);
        }
    }

    public struct float2 
    {
        public float2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public float x { get; set; }
        public float y { get; set; }

        public static implicit operator Vector2(float2 f)
        {
            return new Vector2(f.x, f.y);
        }

        public static implicit operator float2(Vector2 v)
        {
            return new float2(v.x, v.y);
        }
    }

}
