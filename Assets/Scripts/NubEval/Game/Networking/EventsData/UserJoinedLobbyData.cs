using PubnubApi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NubEval.Networking.EventsData
{
    public struct UserJoinedLobbyData
    {
        public UserJoinedLobbyData(UserId userId, UserAccountData accountData)
        {
            UserId = userId;
            AccountData = accountData;
        }

        public UserId UserId { get; set; }
        public UserAccountData AccountData { get; set; }
    }
}
