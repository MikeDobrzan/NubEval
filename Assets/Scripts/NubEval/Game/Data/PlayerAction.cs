using UnityEngine;

namespace NubEval
{
    public struct PlayerAction
    {
        /// <summary>
        /// DestinationPosition
        /// </summary>
        public Vector2 MoveTo { get; set; }

        /// <summary>
        /// Shoot at player ID
        /// </summary>
        public int ShootAt { get; set; }
    }
}
