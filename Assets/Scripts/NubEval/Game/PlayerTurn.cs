using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NubEval.Game.Data;

namespace NubEval.Game
{
    public class PlayerTurn
    {
        private PlayerTurn(bool completed)
        {
            Completed = completed;
        }

        public bool Completed { get; private set; }
        public PlayerAction PlayerAction { get; set; }

        public static PlayerTurn NewTurn()
        {
            return new PlayerTurn(false);
        }

        public void Complete()
        {
            Completed = true;
        }
    }
}
