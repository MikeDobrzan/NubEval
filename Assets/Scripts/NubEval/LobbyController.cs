using PubnubApi;
using System.Collections.Generic;
using UIComponents;
using UnityEngine;

namespace NubEval
{
    public class LobbyController : MonoBehaviour
    {
        [SerializeField] private LobbyPanel uiFriendList;
        [SerializeField] private PNDevice _pubnub;


        public void Construct(PNDevice pn)
        {
            _pubnub = pn;
        }

        public async void OnBoot()
        {
            //get all users in the lobby channel
            var users = await _pubnub.Presence.GetUsersInChannel(Channels.MainChannel);

            List<LobbyUserItemData> uiItems = new List<LobbyUserItemData>();

            foreach (var user in users.Values)
            {
                //get metadata
                var metaData = await _pubnub.DataUsers.GetAccountData(user.ID);

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
    }
}
