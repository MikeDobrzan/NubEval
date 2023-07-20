using NubEval.Networking;
using NubEval.Networking.PubNubWrapper;
using PubnubApi.Unity;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace NubEval
{
    /// <summary>
    /// UserDevice
    /// </summary>
    public class PNDevice : MonoBehaviour
    {
        [SerializeField] private PlayerPrefsAsset playerPrefs;

        private SubscribeCallbackListener _listener;
        private PNPublish _messages;
        private PNSubscribe _subscribe;
        private PNConnection _connection;
        private PNDatastoreUsers _dataUsers;
        private PNPresence _presence;
        private INetworkEventHandler _networkEventHandler;
        private IRemoteLobbyEventsListener _remoteLobbyEventsListener;

        public PNConnection Connection => _connection;
        public PNDatastoreUsers UserData => _dataUsers;
        public PNPublish MessageDispatcher => _messages;
        public PNPresence Presence => _presence;
        public PNSubscribe Subscriptions => _subscribe;
        public IRemoteLobbyEventsListener RemoteEventsLobby => _remoteLobbyEventsListener;

        public async Task<PNDevice> Connect(PNConfigData config)
        {
            Debug.LogWarning($"Try to connect: {playerPrefs.PnUserID}");

            _listener = new SubscribeCallbackListener();
            _connection = new PNConnection(playerPrefs.PnUserID, config);

            var cts = new CancellationTokenSource(5000); //If not connected after 5sec;
            var pubnub = await _connection.SetListener(_listener, cts.Token);

            var networkListener = new NetworkEventsHandler(this, playerPrefs.DeviceData);
            _networkEventHandler = networkListener;
            _remoteLobbyEventsListener = networkListener;

            _subscribe = new PNSubscribe(pubnub);
            _messages = new PNPublish(pubnub);
            _presence = new PNPresence(pubnub);
            _dataUsers = new PNDatastoreUsers(pubnub);

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
            Connection.Disconnect();
        }
    }
}
