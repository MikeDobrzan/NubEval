using UnityEngine;

namespace NubEval
{
    public struct PlayerAction
    {
        public PlayerAction(Vector2 moveTo, bool shoot)
        {
            MoveDir = moveTo;
            Shoot = shoot;
        }

        /// <summary>
        /// DestinationPosition
        /// </summary>
        public Vector2 MoveDir { get; set; }

        /// <summary>
        /// Shoot
        /// </summary>
        public bool Shoot { get; set; }
    }
}
