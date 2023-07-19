using UnityEngine;

namespace NubEval
{
    [CreateAssetMenu(fileName = "PubNubConfigAsset", menuName = "Assets/Config")]
    public class PNConfigDataAsset : ScriptableObject
    {
        [SerializeField] private PNConfigData data;

        public PNConfigData Data { get => data; }
    }
}
