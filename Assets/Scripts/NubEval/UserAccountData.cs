using UnityEngine;

namespace NubEval
{
    [System.Serializable]
    public struct UserAccountData
    {
        [SerializeField] private string gameAccountId; //pubnub id
        [SerializeField] private string pubnubUserId; //pubnub id
        [SerializeField] private string displayName;

        public UserAccountData(string gameAccountId, string pubnubUserId, string displayName)
        {
            this.gameAccountId = gameAccountId;
            this.pubnubUserId = pubnubUserId;
            this.displayName = displayName;
        }

        public string GameAccountId => gameAccountId;
        public string PubNubUserID => pubnubUserId;
        public string DisplayName => displayName;
    }
}
