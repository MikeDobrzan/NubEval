using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NubEval
{
    [System.Serializable]
    public struct PlayerData
    {
        [SerializeField] private string name;
        [SerializeField] private Color color;

        public string Name { get => name; set => name = value; }
        public Color Color { get => color; set => color = value; }
    }
}
