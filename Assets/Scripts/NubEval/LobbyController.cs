using PubnubApi;
using System.Collections.Generic;
using System.Threading.Tasks;
using UIComponents;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

namespace NubEval
{
    public class LobbyController : MonoBehaviour,
        ILobbyEventsSubscriber
    {
        [SerializeField] private LobbyPanel uiFriendList;
        private PNDevice _device;


        public void Construct(PNDevice pn)
        {
            _device = pn;
        }

        public void OnBoot()
        {
            ////get all users in the lobby channel           
            //var cardsData = await GetInitialDataForUI();
            //uiFriendList.Refresh(cardsData);

            _device.RemoteEventsLobby.SubscribeToLobbyEvents(this);
        }

        private async Task<List<LobbyUserItemData>> GetInitialDataForUI()
        {
            List<LobbyUserItemData> uiItems = new List<LobbyUserItemData>();
            
            var users = await _device.Presence.GetUsersInChannel(Channels.DebugChannel);


            foreach (var user in users.Values)
            {
                //get metadata
                var metaData = await _device.MetadataUsers.GetAccountDataAsync(user.ID);

                UserAccountData accountData;

                if (metaData.Item1 == false)
                    accountData = new UserAccountData("null", user.ID, "null");
                else
                    accountData = metaData.Item2;

                //Debug.Log($"in lobby user={user.ID} | ({user.PresenceStates[0].StateType}-{user.PresenceStates[0].State}) | ({accountData.DisplayName})");

                uiItems.Add(new LobbyUserItemData(accountData, user.PresenceStates));
                
            }

            return uiItems;
        }


        void ILobbyEventsSubscriber.OnUserLeave(UserId user, UserAccountData accountData)
        {
            _device.Console.Log($"{user} Left!");
            uiFriendList.RemovePlayerCard(new LobbyUserItemData(accountData, default));
        }

        void ILobbyEventsSubscriber.OnUserJoin(UserId user, UserAccountData accountData, List<PresenceState> states)
        {
            _device.Console.Log($"{accountData.DisplayName} Joined!");
            uiFriendList.AddPlayerCard(new LobbyUserItemData(accountData, states));
        }

        void ILobbyEventsSubscriber.OnUserChangeState(UserId user, UserAccountData accountData, List<PresenceState> states)
        {
            _device.Console.Log($"{user} changed state!");
            uiFriendList.UpdatePlayerCardData(new LobbyUserItemData(accountData, states));
        }
    }
}
