using NubEval.Game.Networking.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace NubEval
{
    public class Bootloader : MonoBehaviour
    {
        [SerializeField] private PNConfigDataAsset configAsset;

        [Header("Services")]
        [SerializeField] private PNDevice aDevA;
        [SerializeField] private PNDevice aDevB;
        [SerializeField] private PNDevice bDevA;
        [SerializeField] private UserManagementInput addUserUI;
        [SerializeField] private LobbyController lobby;

        private async void Start()
        {
            //Initialize PubNub
            await aDevA.Connect(configAsset.Data);
            await aDevB.Connect(configAsset.Data);           
            await bDevA.Connect(configAsset.Data);

            lobby.Construct(aDevA);
            addUserUI.Cosntruct(aDevA);          
            
            List<Channel> channels = new List<Channel>
            {
                Channels.MainChannel,
                //new Channel(Channels.Lobby, ChannelType.PresenceChannel)
            };

            aDevA.Subscriptions.SubscribeChannels(channels);
            aDevB.Subscriptions.SubscribeChannels(channels);
            bDevA.Subscriptions.SubscribeChannels(channels);
            await Task.Delay(3000);

            Debug.Log("Boot Complete!");

            await aDevA.Presence.ChannelJoin(Channels.MainChannel, new PresenceState("lobbyState", "In Game"));
            await bDevA.Presence.ChannelJoin(Channels.MainChannel, new PresenceState("lobbyState", "ffffuuuuuuuuuu"));

            await Task.Delay(3000);
            //var state = await aDevA.Presence.GetStatesCurrentUser(Channels.MainChannel);
            // Publish example
            //(bool, MessageID) bla = await aDevA.MessageDispatcher.SendMsg("Hello World from Unity!", Channels.MainChannel);
            //(bool, MessageID) resp = await aDevA.MessageDispatcher.SendMsg("Join!", Channels.Lobby);

            lobby.OnBoot();
        }


        //Currently gets all users present in the Lobby channel
        private void GetFriends()
        {

        }
    }
}