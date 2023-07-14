using UnityEngine;

namespace NubEval
{
    [System.Serializable]
    public struct UserAccountData
    {
        [SerializeField] private string gameAccountId; //pubnub id
        [SerializeField] private string pubnubUserId; //pubnub id
        [SerializeField] private string displayName;

        public string GameAccountId => gameAccountId;
        public string PubNubUserID => pubnubUserId;
        public string DisplayName => displayName;
    }
}
