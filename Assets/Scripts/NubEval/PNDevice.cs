using NubEval.Game.Networking.Data;
using NubEval.Networking.PubNubWrapper;
using PubnubApi;
using PubnubApi.Unity;
using System.Threading.Tasks;
using UnityEngine;
using System.Threading;

namespace NubEval
{

    /// <summary>
    /// UserDevice
    /// </summary>
    public class PNDevice : MonoBehaviour
    {
        [SerializeField] private PNConfigDataAsset configAsset;
        [SerializeField] private string deviceID;

        private Pubnub _pubnub;
        private SubscribeCallbackListener _listener;

        // UserId identifies this client.
        public UserAccountData account;

        private PNPublish _messages;
        private PNSubscribe _subscribe;
        private PNConnection _connection;
        private PNDatastoreUsers _dataUsers;
        private PNPresence _presence;
        private INetworkEventHandler _networkEventHandler;

        public Pubnub Pubnub => _pubnub;
        public PNConnection Connection => _connection;
        public PNDatastoreUsers DataUsers => _dataUsers;
        public PNPublish MessageDispatcher => _messages;
        public PNPresence Presence => _presence;
        public PNSubscribe Subscriptions => _subscribe;

        public async Task<PNDevice> Init()
        {
            _listener = new SubscribeCallbackListener();
            _connection = new PNConnection(account, configAsset.Data);

            var cts = new CancellationTokenSource(5000); //If not connected after 5sec;
            _pubnub = await _connection.SetListener(_listener, cts.Token);

            _networkEventHandler = new NetworkEventsHandler(deviceID);
            _subscribe = new PNSubscribe(_pubnub);
            _messages = new PNPublish(_pubnub);
            _presence = new PNPresence(_pubnub);
            _dataUsers = new PNDatastoreUsers(_pubnub);

            // Listener example.
            _listener.onStatus += _networkEventHandler.OnPnStatus;
            _listener.onMessage += _networkEventHandler.OnPnMessage;
            _listener.onPresence += _networkEventHandler.OnPnPresence;
            _listener.onFile += _networkEventHandler.OnPnFile;
            _listener.onObject += _networkEventHandler.OnPnObject;
            _listener.onSignal += _networkEventHandler.OnPnSignal;
            _listener.onMessageAction += _networkEventHandler.OnPnMessageAction;

            return this;
        }

       private void OnDestroy()
        {
            _pubnub.UnsibscribeAll();
        }
    }
}