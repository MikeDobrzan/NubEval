using NubEval.Networking.PubNubWrapper;
using PubnubApi;
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
        [SerializeField] private PNConfigDataAsset configAsset;
        [SerializeField] private PlayerPrefsAsset playerPrefs;

        private SubscribeCallbackListener _listener;
        private PNPublish _messages;
        private PNSubscribe _subscribe;
        private PNConnection _connection;
        private PNDatastoreUsers _dataUsers;
        private PNPresence _presence;
        private INetworkEventHandler _networkEventHandler;

        public PNConnection Connection => _connection;
        public PNDatastoreUsers DataUsers => _dataUsers;
        public PNPublish MessageDispatcher => _messages;
        public PNPresence Presence => _presence;
        public PNSubscribe Subscriptions => _subscribe;

        public async Task<PNDevice> Connect()
        {
            _listener = new SubscribeCallbackListener();
            _connection = new PNConnection(playerPrefs.PnUserID, configAsset.Data);

            var cts = new CancellationTokenSource(5000); //If not connected after 5sec;
            var pubnub = await _connection.SetListener(_listener, cts.Token);

            _networkEventHandler = new NetworkEventsHandler(playerPrefs.DeviceData);
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
