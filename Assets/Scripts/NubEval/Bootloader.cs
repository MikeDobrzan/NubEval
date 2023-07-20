using NubEval.Game.Networking.Data;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.tvOS;

namespace NubEval
{
    public class Bootloader : MonoBehaviour
    {
        [SerializeField] private PNConfigDataAsset configAsset;

        [Header("Services")]
        [SerializeField] private PlayerPrefsAsset DevAPlayerPrefs;
        [SerializeField] private PlayerPrefsAsset DevBPlayerPrefs;
        [SerializeField] private UserManagementInput addUserUI;
        [SerializeField] private LobbyController lobby;

        private PNDevice devA;
        private PNDevice devB;

        private async void Start()
        {
            var cts = new CancellationTokenSource(5000); //If not connected after 5sec;

            //Initialize PubNub
            devA = new PNDevice(configAsset.Data, DevAPlayerPrefs.PnUserID, DevAPlayerPrefs.DeviceData);
            await devA.Connection.Connect(cts.Token);

            JoinLobby();

            lobby.Construct(devA);
            addUserUI.Cosntruct(devA);          
            
            //List<Channel> channels = new List<Channel>
            //{
            //    Channels.MainChannel,
            //    //new Channel(Channels.Lobby, ChannelType.PresenceChannel)
            //};

            //devA.Subscriptions.SubscribeChannels(channels);
            //await Task.Delay(1000);
            //devB.Subscriptions.SubscribeChannels(channels);
            //await Task.Delay(1000);

            ////await devA.Presence.SetPresenceState(Channels.MainChannel, new PresenceState("lobbyState", "In Lobby"));
            ////await devB.Presence.SetPresenceState(Channels.MainChannel, new PresenceState("lobbyState", "In Lobby"));                       

            Debug.Log("Boot Complete!");

            //await Task.Delay(3000);
            ////var state = await aDevA.Presence.GetStatesCurrentUser(Channels.MainChannel);
            //// Publish example
            ////(bool, MessageID) bla = await aDevA.MessageDispatcher.SendMsg("Hello World from Unity!", Channels.MainChannel);
            ////(bool, MessageID) resp = await aDevA.MessageDispatcher.SendMsg("Join!", Channels.Lobby);

            lobby.OnBoot();
            //somethign
        }

        private async void JoinLobby()
        {
            await devA.Presence.SetPresenceState(Channels.DebugChannel, new PresenceState("lobbyState", "In Lobby"));
            devA.RemoteEventsLobby.SubscribeLobbyEvents(lobby);
        }

        //Currently gets all users present in the Lobby channel
        private void GetFriends()
        {

        }

        private void OnDestroy()
        {
            devA?.Dispose();
            devB?.Dispose();
        }
    }
}