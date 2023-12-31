using PubnubApi;
using UnityEngine;

namespace NubEval.Game.Networking
{
    [System.Serializable]
    public struct UserAccountData
    {
        [SerializeField] private string gameAccountId; //pubnub id
        [SerializeField] private string pubnubUserId; //pubnub id
        [SerializeField] private string displayName;
        //[SerializeField] private string deviceID;

        public UserAccountData(string gameAccountId, string pubnubUserId, string displayName)
        {
            this.gameAccountId = gameAccountId;
            this.pubnubUserId = pubnubUserId;
            this.displayName = displayName;
        }

        public UserAccountData(UserId pubnubUserId)
        {
            this.gameAccountId = "guest_account";
            this.pubnubUserId = pubnubUserId;
            this.displayName = "Guest";
        }

        public string GameAccountId => gameAccountId;
        public string PubNubUserID => pubnubUserId;
        public string DisplayName => displayName;
    }
}
