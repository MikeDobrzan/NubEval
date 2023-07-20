using PubnubApi;
using System.Collections.Generic;
using UIComponents;
using UnityEngine;

namespace NubEval
{
    public class LobbyController : MonoBehaviour,
        ILobbyEventsHandler
    {
        [SerializeField] private LobbyPanel uiFriendList;
        private PNDevice _remote;


        public void Construct(PNDevice pn)
        {
            _remote = pn;
        }

        public async void OnBoot()
        {
            //get all users in the lobby channel
            var users = await _remote.Presence.GetUsersInChannel(Channels.MainChannel);

            List<LobbyUserItemData> uiItems = new List<LobbyUserItemData>();

            foreach (var user in users.Values)
            {
                //get metadata
                var metaData = await _remote.UserData.GetAccountDataAsync(user.ID);

                UserAccountData accountData;

                if (metaData.Item1 == false)
                    accountData = new UserAccountData("null", user.ID, "null");
                else
                    accountData = metaData.Item2;

                //Debug.Log($"in lobby user={user.ID} | ({user.PresenceStates[0].StateType}-{user.PresenceStates[0].State}) | ({accountData.DisplayName})");

                uiItems.Add(new LobbyUserItemData(accountData, user.PresenceStates));
            }

            uiFriendList.Refresh(uiItems);
        }

        void ILobbyEventsHandler.OnUserJoined(UserId user, UserAccountData accountData)
        {
            Debug.Log($"{accountData.DisplayName} Joined!");
        }
    }
}
