using NubEval.Networking.PubNubWrapper;
using NubEval.Networking;
using PubnubApi;
using PubnubApi.Unity;
using System;
using UnityEditor.VersionControl;
using UnityEngine;
using System.Collections.Generic;

namespace NubEval
{
    /// <summary>
    /// UserDevice
    /// </summary>
    public class PNDevice : PNManagerBehaviour
    {
        // UserId identifies this client.
        public string userId;

        private UserDeviceData DeviceData => new UserDeviceData($"dev:{userId}", DeviceType.Mobile);

        private async void Awake()
        {
            ConstructPNDevice(default, userId, default);
        }


        private void OnPnPresence(Pubnub pn, PNPresenceEventResult result)
        {
            Console.Log($"PresenceEventReceived | Same API???={pubnub == pn}");
        }

        private void OnPnStatus(Pubnub pn, PNStatus status)
        {
            Debug.Log(status.Category == PNStatusCategory.PNConnectedCategory ? "Connected" : "Not connected");
        }

        public PNConnection Connection => null;
        public PNDatastoreUsers UserData => _dataUsers;
        public PNPublish MessageDispatcher => _messages;
        public PNPresence Presence => _presence;
        public PNSubscribe Subscriptions => _subscribe;
        public IRemoteLobbyEventsListener RemoteEventsLobby => _networkEventsHandler;
        public INetworkEventHandler NetworkEventsHandler => _networkEventsHandler;
        

        private SubscribeCallbackListener _listener;
        //private Pubnub _pubnub;
        private PNPublish _messages;
        private PNSubscribe _subscribe;
        private PNConnection _connection;
        private PNDatastoreUsers _dataUsers;
        private PNPresence _presence;
        private PNDeviceConsole _console;
        private NetworkEventsHandler _networkEventsHandler;

        public PNDeviceConsole Console => _console;
        
        
        public async void ConstructPNDevice(PNConfigData config, UserId userId, UserDeviceData deviceData)
        {
            if (string.IsNullOrEmpty(userId))
            {
                // It is recommended to change the UserId to a meaningful value, to be able to identify this client.
                userId = System.Guid.NewGuid().ToString();
            }

            _networkEventsHandler = new NetworkEventsHandler(this, DeviceData);

            // Listener example.
            listener.onStatus += NetworkEventsHandler.OnPnStatus;
            listener.onMessage += NetworkEventsHandler.OnPnMessage;
            listener.onPresence += NetworkEventsHandler.OnPnPresence;
            listener.onFile += NetworkEventsHandler.OnPnFile;
            listener.onObject += NetworkEventsHandler.OnPnObject;
            listener.onSignal += NetworkEventsHandler.OnPnSignal;
            listener.onMessageAction += NetworkEventsHandler.OnPnMessageAction;

            Initialize(userId);

            _console = new PNDeviceConsole(pubnub, this, DeviceData);
            _messages = new PNPublish(pubnub, this);
            _subscribe = new PNSubscribe(pubnub, this);
            _presence = new PNPresence(pubnub, this);
            _dataUsers = new PNDatastoreUsers(pubnub);


            //Handshake
            Subscriptions.SubscribeChannels(new List<Channel> { Channels.DebugChannel });
            await MessageDispatcher.SendMsg("Handshake", Channels.DebugChannel);
        }        

        private PNConfiguration CompileApiConfig(UserId _userId, PNConfigData config)
        {
            PNConfiguration pnConfiguration = new PNConfiguration(_userId);
            pnConfiguration.SubscribeKey = config.SubscribeKey;
            pnConfiguration.PublishKey = config.PublishKey;
            //pnConfiguration.UserId = _userId;

            return pnConfiguration;
        }

        public void Clean()
        {
            //Connection.Disconnect();
            pubnub.UnsibscribeAll();
        }

        protected override void OnDestroy()
        {
            // Use OnDestroy to clean up, e.g. unsubscribe from listeners.
            listener.onStatus -= NetworkEventsHandler.OnPnStatus;
            listener.onMessage -= NetworkEventsHandler.OnPnMessage;
            listener.onPresence -= NetworkEventsHandler.OnPnPresence;
            listener.onFile -= NetworkEventsHandler.OnPnFile;
            listener.onObject -= NetworkEventsHandler.OnPnObject;
            listener.onSignal -= NetworkEventsHandler.OnPnSignal;
            listener.onMessageAction -= NetworkEventsHandler.OnPnMessageAction;

            base.OnDestroy();
        }
    }
}