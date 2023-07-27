using UnityEngine;

namespace NubEval.PubNubWrapper.ScriptableObjects
{
    [CreateAssetMenu(fileName = "PubNubConfigAsset", menuName = "Assets/Config")]
    public class PNConfigDataAsset : ScriptableObject
    {
        [SerializeField] private PNConfigData data;

        public PNConfigData Data { get => data; }
    }
}
