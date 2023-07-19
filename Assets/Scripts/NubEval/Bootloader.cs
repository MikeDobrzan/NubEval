using NubEval.Game.Networking.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace NubEval
{
    public class Bootloader : MonoBehaviour
    {
        [Header("Services")]
        [SerializeField] private PNWrapper pnWrapper;
        [SerializeField] private AddUserController addUserUI;
        [SerializeField] private LobbyController lobby;

        private async void Start()
        {
            //Initialize PubNub
            var pn = await pnWrapper.Init();
            Debug.Log("Boot Complete!");
            await Task.Delay(1000);
            lobby.Construct(pn);

            List<Channel> channels = new List<Channel>
            {
                new Channel(Channels.MainChannel, ChannelType.PresenceChannel),
                new Channel(Channels.Lobby, ChannelType.PresenceChannel)
            };

            pn.Subscriptions.SubscribeChannels(channels);

            await Task.Delay(500);
            await pn.Presence.ChannelJoin(Channels.MainChannel, new PresenceState("lobbyState", "idle"));
            await Task.Delay(1000);


            var state = await pnWrapper.Presence.GetStatesCurrentUser(Channels.MainChannel);
            Debug.Log(state[0].State);

            // Publish example
            (bool, MessageID) bla = await pnWrapper.MessageDispatcher.SendMsg("Hello World from Unity!", Channels.MainChannel);
            (bool, MessageID) resp = await pnWrapper.MessageDispatcher.SendMsg("Join!", Channels.Lobby);

            addUserUI.Cosntruct(pnWrapper);

            lobby.OnBoot();
        }


        //Currently gets all users present in the Lobby channel
        private void GetFriends()
        {

        }
    }
}