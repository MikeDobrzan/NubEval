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
        private PNDevice _device;


        public void Construct(PNDevice pn)
        {
            _device = pn;
        }

        public async void OnBoot()
        {
            //get all users in the lobby channel
            var users = await _device.Presence.GetUsersInChannel(Channels.DebugChannel);

            List<LobbyUserItemData> uiItems = new List<LobbyUserItemData>();

            foreach (var user in users.Values)
            {
                //get metadata
                var metaData = await _device.UserData.GetAccountDataAsync(user.ID);

                UserAccountData accountData;

                if (metaData.Item1 == false)
                    accountData = new UserAccountData("null", user.ID, "null");
                else
                    accountData = metaData.Item2;

                //Debug.Log($"in lobby user={user.ID} | ({user.PresenceStates[0].StateType}-{user.PresenceStates[0].State}) | ({accountData.DisplayName})");

                uiItems.Add(new LobbyUserItemData(accountData, user.PresenceStates));
            }

            uiFriendList.Refresh(uiItems);

            _device.RemoteEventsLobby.SubscribeToLobbyEvents(this);
        }

        void ILobbyEventsHandler.OnUserLeave(UserId user, UserAccountData accountData)
        {
            _device.Console.Log($"{user} Left!");
            uiFriendList.RemovePlayerCard(new LobbyUserItemData(accountData, default));
        }

        void ILobbyEventsHandler.OnUserJoin(UserId user, UserAccountData accountData, List<PresenceState> states)
        {
            _device.Console.Log($"{accountData.DisplayName} Joined!");
            uiFriendList.AddPlayerCard(new LobbyUserItemData(accountData, states));
        }

        void ILobbyEventsHandler.OnUserChangeState(UserId user, List<PresenceState> states)
        {
            _device.Console.Log($"{user} changed state!");
        }
    }
}
