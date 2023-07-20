using UnityEngine;

namespace NubEval
{
    /// <summary>
    /// PubNub API config data
    /// </summary>
    [System.Serializable]
    public struct PNConfigData
    {
        [SerializeField] private string publishKey;
        [SerializeField] private string subscribeKey;

        public PNConfigData(string publishKey, string subscribeKey)
        {
            this.publishKey = publishKey;
            this.subscribeKey = subscribeKey;
        }

        public string PublishKey { get => publishKey; }
        public string SubscribeKey { get => subscribeKey; }
    }
}