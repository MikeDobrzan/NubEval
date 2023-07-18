using NubEval;
using System.Collections.Generic;
using UnityEngine;

namespace UIComponents
{
    [System.Serializable]
    public struct LobbyUserItemData
    {
        [SerializeField] private UserAccountData accountData;
        [SerializeField] private List<PresenceState> states;

        public LobbyUserItemData(UserAccountData accountData, List<PresenceState> states)
        {
            this.accountData = accountData;
            this.states = states;
        }

        public string UserID => accountData.PubNubUserID;
        public string DisplayName => accountData.DisplayName;
        public string PlayingState
        {
            get
            {
                return states.Find(s => s.StateType == "lobbyState").State;
            }
        }
    }
}
