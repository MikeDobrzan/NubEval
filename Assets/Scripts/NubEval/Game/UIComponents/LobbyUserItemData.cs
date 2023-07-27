using NubEval.Game.Networking;
using System.Collections.Generic;
using UnityEngine;

namespace NubEval.Game.UIComponents
{
    [System.Serializable]
    public struct LobbyUserItemData
    {
        [SerializeField] private UserAccountData accountData;
        [SerializeField] private List<PresenceState> states;

        public LobbyUserItemData(UserAccountData accountData, List<PresenceState> states)
        {
            this.accountData = accountData;
            this.states = new List<PresenceState>();
            if (states != null)
                this.states.AddRange(states);
            else
                Debug.LogWarning("EmptyStates!");
        }

        public string UserID => accountData.PubNubUserID;
        public string DisplayName => accountData.DisplayName;

        public string MatchState
        {
            get
            {
                return states.Find(s => s.StateType == StateType.matchState).State;
            }
        }

        public string LobbyState
        {
            get
            {
                return states.Find(s => s.StateType == StateType.lobbyState).State;
            }
        }
    }
}
