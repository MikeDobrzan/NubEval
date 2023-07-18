using NubEval.Game.Networking.Data;
using NubEval.Networking.PubNubWrapper;
using PubnubApi;
using PubnubApi.Unity;
using System.Threading.Tasks;
using UnityEngine;

namespace NubEval
{
    public class PNWrapper : MonoBehaviour
    {
        [SerializeField] private string message;
        [SerializeField] private string channel;
        [SerializeField] private Match testMatch;
        [SerializeField] private PNConfigAsset config;

        [SerializeField] private MessageID lastMsg;

        private Pubnub _pubnub;
        private SubscribeCallbackListener _listener;

        // UserId identifies this client.
        public string userId;

        private PNMessenger _messages;
        private PNConnection _connection;
        private PNDatastoreUsers _dataUsers;
        private PNPresence _presence;
        private INetworkEventHandler _networkEventHandler;

        public Pubnub Pubnub => _pubnub;
        public PNConnection Connection => _connection;
        public PNDatastoreUsers DataUsers => _dataUsers;
        public PNMessenger MessageDispatcher => _messages;
        public PNPresence Presence => _presence;

        public async Task<PNWrapper> Init()
        {
            _listener = new SubscribeCallbackListener();
            _connection = new PNConnection(userId, config);
            _pubnub = _connection.ConnectListener(_listener);
            _networkEventHandler = new NetworkEventsHandler();

            _messages = new PNMessenger(_pubnub);
            _presence = new PNPresence(_pubnub, userId);
            _dataUsers = new PNDatastoreUsers(_pubnub, userId);

            // Listener example.
            _listener.onStatus += _networkEventHandler.OnPnStatus;
            _listener.onMessage += _networkEventHandler.OnPnMessage;
            _listener.onPresence += _networkEventHandler.OnPnPresence;
            _listener.onFile += _networkEventHandler.OnPnFile;
            _listener.onObject += _networkEventHandler.OnPnObject;
            _listener.onSignal += _networkEventHandler.OnPnSignal;
            _listener.onMessageAction += _networkEventHandler.OnPnMessageAction;

            _pubnub.Subscribe<string>().Channels(new[] { "Channel-Barcelona" }).Execute();
            await Task.Delay(1000);
            //_pubnub.Subscribe<string>().Channels(new[] { "ch3" }).Execute();
            await Task.Delay(1000);
            _pubnub.Subscribe<string>().Channels(new[] { Channels.Lobby }).Execute();



            return this;
        }

        public async void SendNetworkObject()
        {
            (bool, MessageID) msg = await MessageDispatcher.SendMsg(testMatch, Channels.MainChannel);

            if (msg.Item1 == false)
                return;
            else
                lastMsg = msg.Item2;
        }

        public async void SendMsg()
        {
            await SendMsg(message, channel);
        }

        private async Task SendMsg(string message, string channel)
        {
            (bool, MessageID) msg = await MessageDispatcher.SendMsg(message, channel);

            if (msg.Item1 == false)
                return;
            else
                lastMsg = msg.Item2;
        }

        private void OnDestroy()
        {
            _pubnub.UnsibscribeAll();
        }
    }
}
