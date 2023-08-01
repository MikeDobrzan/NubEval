using NubEval.Game.Networking;
using PubnubApi;
using PubnubApi.Unity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace NubEval.PubNubWrapper
{
    /// <summary>
    /// UserDevice is a wrapper for the PubNub Api
    /// </summary>
    public class PNDevice : IDisposable
    {
        private readonly Pubnub _pnApi;
        private readonly PNPublish _messages;
        private readonly PNSubscribe _subscribe;
        private readonly PNDatastoreUsers _dataUsers;
        private readonly PNDatastoreChannels _dataChannels;
        private readonly PNPresence _presence;
        private readonly PNDeviceConsole _console;
        private readonly NetworkEventsHandler _networkEventsHandler;
        private readonly UserDeviceData _deviceData;
        private readonly bool isGlobalListener;

        private SubscribeCallbackListener _listener;

        public PNDevice(PNConfigData config, UserId userId, UserDeviceData deviceData)
        {
            _deviceData = deviceData;
            _pnApi = new Pubnub(CompileApiConfig(userId, config));

            _networkEventsHandler = new NetworkEventsHandler(_pnApi, this, DeviceData);
            _console = new PNDeviceConsole(_pnApi, this, DeviceData);
            _messages = new PNPublish(_pnApi, this);
            _subscribe = new PNSubscribe(_pnApi, this);
            _presence = new PNPresence(_pnApi, this);
            _dataUsers = new PNDatastoreUsers(_pnApi);
            _dataChannels = new PNDatastoreChannels(_pnApi, this);
        }

        private Pubnub PnApi => _pnApi;

        public SubscribeCallbackListener Listener => _listener;
        public UserDeviceData DeviceData => _deviceData;
        public PNDatastoreUsers MetadataUsers => _dataUsers;
        public PNDatastoreChannels MetadataChannels => _dataChannels;
        public PNPublish MessageDispatcher => _messages;
        public PNPresence Presence => _presence;
        public PNSubscribe Subscriptions => _subscribe;
        public IRemoteLobbyEventsListener RemoteEventsLobby => _networkEventsHandler;
        public IMatchEventsListener RemoteEventsMatch => _networkEventsHandler;
        public INetworkEventHandler NetworkEventsHandler => _networkEventsHandler;
        public PNDeviceConsole Console => _console;

        public void SetListener(SubscribeCallbackListener listener)
        {
            if (listener == null)
            {
                Debug.LogError("Listenr Null!");
                return;
            }

            if (Listener != null)
                DisconnectListener();

            _listener = listener;

            Listener.onStatus += NetworkEventsHandler.OnPnStatus;
            Listener.onMessage += NetworkEventsHandler.OnPnMessage;
            Listener.onPresence += NetworkEventsHandler.OnPnPresence;
            Listener.onFile += NetworkEventsHandler.OnPnFile;
            Listener.onObject += NetworkEventsHandler.OnPnObject;
            Listener.onSignal += NetworkEventsHandler.OnPnSignal;
            Listener.onMessageAction += NetworkEventsHandler.OnPnMessageAction;

            _pnApi.AddListener(Listener);
        }

        public async Task Connect()
        {
            //Handshake
            await Subscriptions.SubscribeChannels(new List<Channel> { Channels.Connection });
        }

        public async Task Disconnect()
        {
            DisconnectListener();
            Dispose();
            await Task.Delay(1000);
        }

        public void Dispose()
        {
            _pnApi.UnsubscribeAll();
        }

        private void DisconnectListener()
        {
            Listener.onStatus -= NetworkEventsHandler.OnPnStatus;
            Listener.onMessage -= NetworkEventsHandler.OnPnMessage;
            Listener.onPresence -= NetworkEventsHandler.OnPnPresence;
            Listener.onFile -= NetworkEventsHandler.OnPnFile;
            Listener.onObject -= NetworkEventsHandler.OnPnObject;
            Listener.onSignal -= NetworkEventsHandler.OnPnSignal;
            Listener.onMessageAction -= NetworkEventsHandler.OnPnMessageAction;
        }

        private PNConfiguration CompileApiConfig(UserId _userId, PNConfigData config)
        {
            PNConfiguration pnConfiguration = new PNConfiguration(_userId);
            pnConfiguration.SubscribeKey = config.SubscribeKey;
            pnConfiguration.PublishKey = config.PublishKey;
            pnConfiguration.SetPresenceTimeoutWithCustomInterval(10, 5);

            return pnConfiguration;
        }
    }
}