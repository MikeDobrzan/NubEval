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
        [SerializeField] private Match testMatch;
        [SerializeField] private PNConfigAsset config;

        [SerializeField] private MessageID lastMsg;

        private Pubnub _pubnub;
        private SubscribeCallbackListener _listener = new SubscribeCallbackListener();

        // UserId identifies this client.
        public string userId;

        private PNMessenger _messages;
        private PNConnection _connection;
        private PNDatastoreUsers _dataUsers;
        private INetworkEventHandler _networkEventHandler;

        public Pubnub Pubnub => _pubnub;
        public PNConnection Connection => _connection;
        public PNDatastoreUsers DataUsers => _dataUsers;

        public async Task Init()
        {
            _connection = new PNConnection(userId, config);
            _pubnub = _connection.Connect(_listener);
            _messages = new PNMessenger(_pubnub);
            _networkEventHandler = new NetworkEventsHandler();
            _dataUsers = new PNDatastoreUsers(_pubnub, userId);


            // Listener example.
            _listener.onStatus += _networkEventHandler.OnPnStatus;
            _listener.onMessage += _networkEventHandler.OnPnMessage;
            _listener.onPresence += _networkEventHandler.OnPnPresence;
            _listener.onFile += _networkEventHandler.OnPnFile;
            _listener.onObject += _networkEventHandler.OnPnObject;
            _listener.onSignal += _networkEventHandler.OnPnSignal;
            _listener.onMessageAction += _networkEventHandler.OnPnMessageAction;

            // Subscribe example
            _pubnub.Subscribe<string>().Channels(new[] { Channels.MainChannel }).Execute();

            // Publish example
            var bla = await _messages.SendMsg("Hello World from Unity!", Channels.MainChannel);

            await Task.Delay(2000);
        }

        public async void SendNetworkObject()
        {
            (bool, MessageID) msg = await _messages.SendMsg(testMatch, Channels.MainChannel);

            if (msg.Item1 == false)
                return;
            else
                lastMsg = msg.Item2;
        }

        public async void SendMsg()
        {
            (bool, MessageID) msg = await _messages.SendMsg(message, Channels.MainChannel);

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
