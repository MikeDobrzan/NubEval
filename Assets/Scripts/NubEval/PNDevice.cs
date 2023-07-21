using NubEval.Networking;
using NubEval.Networking.PubNubWrapper;
using PubnubApi;
using PubnubApi.Unity;
using System;

namespace NubEval
{
    /// <summary>
    /// UserDevice
    /// </summary>
    public class PNDevice : IDisposable
    {
        private readonly SubscribeCallbackListener _listener;
        private readonly PNPublish _messages;
        private readonly PNSubscribe _subscribe;
        private readonly PNConnection _connection;
        private readonly PNDatastoreUsers _dataUsers;
        private readonly PNPresence _presence;
        private readonly PNDeviceConsole _console;
        private readonly NetworkEventsHandler _networkEventsHandler;

        public PNConnection Connection => _connection;
        public PNDatastoreUsers UserData => _dataUsers;
        public PNPublish MessageDispatcher => _messages;
        public PNPresence Presence => _presence;
        public PNSubscribe Subscriptions => _subscribe;
        public IRemoteLobbyEventsListener RemoteEventsLobby => _networkEventsHandler;
        public INetworkEventHandler NetworkEventsHandler => _networkEventsHandler;
        public PNDeviceConsole Console => _console;      

        public PNDevice(PNConfigData config, UserId PnUserID, UserDeviceData deviceData)
        {
            _listener = new SubscribeCallbackListener();
            _connection = new PNConnection(PnUserID, config, _listener, out Pubnub pubnub);
            
            _networkEventsHandler = new NetworkEventsHandler(this, deviceData);

            _subscribe = new PNSubscribe(pubnub, this);
            _messages = new PNPublish(pubnub, this);
            _presence = new PNPresence(pubnub, this);
            _dataUsers = new PNDatastoreUsers(pubnub);
            _console = new PNDeviceConsole(pubnub, this, deviceData);

            // Listener example.
            _listener.onStatus += NetworkEventsHandler.OnPnStatus;
            _listener.onMessage += NetworkEventsHandler.OnPnMessage;
            _listener.onPresence += NetworkEventsHandler.OnPnPresence;
            _listener.onFile += NetworkEventsHandler.OnPnFile;
            _listener.onObject += NetworkEventsHandler.OnPnObject;
            _listener.onSignal += NetworkEventsHandler.OnPnSignal;
            _listener.onMessageAction += NetworkEventsHandler.OnPnMessageAction;
        }

        public void Dispose()
        {
            Connection.Disconnect();
        }
    }
}
