using NubEval.Networking;
using NubEval.Networking.PubNubWrapper;
using PubnubApi;
using PubnubApi.Unity;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace NubEval
{
    /// <summary>
    /// UserDevice
    /// </summary>
    public class PNDevice : MonoBehaviour
    {
        public PNConfigDataAsset pnConfigDataAsset;
        public string userId;

        private Pubnub _pnApi;
        private SubscribeCallbackListener _listener;
        private PNPublish _messages;
        private PNSubscribe _subscribe;       
        private PNDatastoreUsers _dataUsers;
        private PNPresence _presence;
        private PNDeviceConsole _console;
        private NetworkEventsHandler _networkEventsHandler;
        private UserDeviceData _deviceData;
        private PNConnection _connection;

        public SubscribeCallbackListener Listener => _listener;
        private UserDeviceData DeviceData => _deviceData;        
        public PNDatastoreUsers UserData => _dataUsers;
        public PNPublish MessageDispatcher => _messages;
        public PNPresence Presence => _presence;
        public PNSubscribe Subscriptions => _subscribe;
        public IRemoteLobbyEventsListener RemoteEventsLobby => _networkEventsHandler;
        public INetworkEventHandler NetworkEventsHandler => _networkEventsHandler;
        public PNDeviceConsole Console => _console;
        public PNConnection Connection => null;

        private async void Awake()
        {
            Initialize(pnConfigDataAsset.Data, userId, new UserDeviceData($"dev:{userId}", DeviceType.Mobile));
            await Hadshake();
        }

        public void Initialize(PNConfigData config, UserId userId, UserDeviceData deviceData)
        {
            _deviceData = deviceData;
            _listener = new SubscribeCallbackListener();
            _networkEventsHandler = new NetworkEventsHandler(this, DeviceData);

            Listener.onStatus += NetworkEventsHandler.OnPnStatus;
            Listener.onMessage += NetworkEventsHandler.OnPnMessage;
            Listener.onPresence += NetworkEventsHandler.OnPnPresence;
            Listener.onFile += NetworkEventsHandler.OnPnFile;
            Listener.onObject += NetworkEventsHandler.OnPnObject;
            Listener.onSignal += NetworkEventsHandler.OnPnSignal;
            Listener.onMessageAction += NetworkEventsHandler.OnPnMessageAction;

            _pnApi = new Pubnub(CompileApiConfig(userId, config));
            _pnApi.AddListener(Listener);

            _console = new PNDeviceConsole(_pnApi, this, DeviceData);
            _messages = new PNPublish(_pnApi, this);
            _subscribe = new PNSubscribe(_pnApi, this);
            _presence = new PNPresence(_pnApi, this);
            _dataUsers = new PNDatastoreUsers(_pnApi);
        }

        public async Task Hadshake()
        {
            //Handshake
            await Subscriptions.SubscribeChannels(new List<Channel> { Channels.Connection, Channels.DebugChannel });
            //await MessageDispatcher.SendMsg("Handshake", Channels.DebugChannel);
        }

        private PNConfiguration CompileApiConfig(UserId _userId, PNConfigData config)
        {
            PNConfiguration pnConfiguration = new PNConfiguration(_userId);
            pnConfiguration.SubscribeKey = config.SubscribeKey;
            pnConfiguration.PublishKey = config.PublishKey;

            return pnConfiguration;
        }

        public void Clean()
        {
            Listener.onStatus -= NetworkEventsHandler.OnPnStatus;
            Listener.onMessage -= NetworkEventsHandler.OnPnMessage;
            Listener.onPresence -= NetworkEventsHandler.OnPnPresence;
            Listener.onFile -= NetworkEventsHandler.OnPnFile;
            Listener.onObject -= NetworkEventsHandler.OnPnObject;
            Listener.onSignal -= NetworkEventsHandler.OnPnSignal;
            Listener.onMessageAction -= NetworkEventsHandler.OnPnMessageAction;

            _pnApi.UnsubscribeAll<string>();
        }

        protected void OnDestroy()
        {
            Clean();
        }
    }
}