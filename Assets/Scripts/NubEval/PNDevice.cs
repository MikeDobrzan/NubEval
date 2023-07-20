using NubEval.Networking;
using NubEval.Networking.PubNubWrapper;
using PubnubApi;
using PubnubApi.Unity;
using System;
using UnityEngine;

namespace NubEval
{
    /// <summary>
    /// UserDevice
    /// </summary>
    public class PNDevice : IDisposable
    {
        //[SerializeField] private PlayerPrefsAsset playerPrefs;

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

        private readonly NetworkEventsHandler _networkEventsHandler;

        public PNDevice(PNConfigData config, UserId PnUserID, UserDeviceData deviceData)
        {
            _listener = new SubscribeCallbackListener();
            _connection = new PNConnection(PnUserID, config, _listener, out Pubnub pubnub);
            _networkEventsHandler = new NetworkEventsHandler(this, deviceData);

            _networkEventHandler = _networkEventsHandler;
            _remoteLobbyEventsListener = _networkEventsHandler;

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
        }

        public void Dispose()
        {
            Connection.Disconnect();
        }
    }
}
