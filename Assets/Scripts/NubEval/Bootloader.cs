using NubEval.Game.Networking.Data;
using PubnubApi.Unity;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.tvOS;
using static PubnubApi.Unity.PubnubExtensions;

namespace NubEval
{
    public class Bootloader : MonoBehaviour
    {
        [SerializeField] private PNConfigDataAsset configAsset;

        [Header("Services")]
        [SerializeField] private PlayerPrefsAsset DevAPlayerPrefs;
        //[SerializeField] private PlayerPrefsAsset DevBPlayerPrefs;
        [SerializeField] private UserManagementInput addUserUI;
        [SerializeField] private LobbyController lobby;

        [SerializeField] private PNDevice devA;
        [SerializeField] private PNDevice devB;
        //private PNDevice devB;

        private async void Awake()
        {
            var cts = new CancellationTokenSource(5000); //If not connected after 5sec;

            devA.Init();
            devB.Init();

            var listener = new SubscribeCallbackListener();

            await devA.ConnectListener(listener);
            await devB.ConnectListener(listener);

            //Initialize PubNub
            //devA.ConstructPNDevice(configAsset.Data, DevAPlayerPrefs.PnUserID, DevAPlayerPrefs.DeviceData);
            //devB = new PNDevice(configAsset.Data, DevBPlayerPrefs.PnUserID, DevBPlayerPrefs.DeviceData);

            //devA.ConnectListener();// .con .Connection.Connect(cts.Token);

            //devA.Subscriptions.SubscribeChannels(new List<Channel> { Channels.DebugChannel });

            //await devA.Presence.SetPresenceState(Channels.DebugChannel, new PresenceState("lobbyState", "In Lobby"));
            //Debug.LogError("wait 10 sec to join Dev B");
            //await Task.Delay(10000);


            //devB.ConnectListener(); //await devB.Connection.Connect(cts.Token);
            //await devB.Presence.SetPresenceState(Channels.DebugChannel, new PresenceState("lobbyState", "In Lobby"));
            ////devB.RemoteEventsLobby.SubscribeLobbyEvents(lobby);
            //Debug.LogError("wait 10 sec to connect Dev A to another channel");
            //await Task.Delay(10000);

            //devA.Subscriptions.SubscribeChannels(new List<Channel> { Channels.DebugChannel } );


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

            //lobby.Construct(devA);
            //lobby.OnBoot();
            //addUserUI.Cosntruct(devA);
        }

        private void OnDestroy()
        {
            //devB?.Dispose();
        }
    }
}