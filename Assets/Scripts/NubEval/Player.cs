using NubEval.Game.Networking;
using NubEval.Game.Networking.Data;
using PubnubApi;
using PubnubApi.Unity;
using System;
using UnityEngine;

namespace NubEval
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private string channel;
        [SerializeField] private string message;
        [SerializeField] private PNConfigAsset config;

        //[SerializeField] private long lastMsgTimetoken;
        [SerializeField] private MessageID lastMsg;

        private Pubnub _pubnub;
        private SubscribeCallbackListener _listener = new SubscribeCallbackListener();

        // UserId identifies this client.
        public string userId;

        private PNMessenger messenger;

        public Pubnub Pubnub => _pubnub;

        private async void Awake()
        {
            if (string.IsNullOrEmpty(userId))
            {
                // It is recommended to change the UserId to a meaningful value, to be able to identify this client.
                userId = System.Guid.NewGuid().ToString();
            }

            // Listener example.
            _listener.onStatus += OnPnStatus;
            _listener.onMessage += OnPnMessage;
            _listener.onPresence += OnPnPresence;
            _listener.onFile += OnPnFile;
            _listener.onObject += OnPnObject;
            _listener.onSignal += OnPnSignal;
            _listener.onMessageAction += OnPnMessageAction;

            messenger = new PNMessenger(_pubnub);

            // Initialize will create a PubNub instance, pass the configuration object, and prepare the listener. 
            _pubnub = Initialize(userId);

            // Subscribe example
            _pubnub.Subscribe<string>().Channels(new[] { Channels.MainChannel }).Execute();

            // Publish example
            var bla = await _pubnub.Publish().Channel(Channels.MainChannel).Message("Hello World from Unity!").ExecuteAsync();
        }

        public Pubnub Initialize(string userId)
        {
            if (Application.isPlaying)
            {
                DontDestroyOnLoad(gameObject);
            }

            UserId user = new UserId(userId);
            PNConfiguration pnConfiguration = new PNConfiguration(user);
            pnConfiguration.SubscribeKey = config.SubscribeKey;
            pnConfiguration.PublishKey = config.PublishKey;

            if (pnConfiguration is null)
            {
                Debug.LogError("PNConfigAsset is missing", this);
                return null;
            }

            if (_pubnub is not null)
            {
                Debug.LogError("PubNub has already been initialized");
                return _pubnub;
            }

            pnConfiguration.UserId = userId;
            _pubnub = new Pubnub(pnConfiguration);
            _pubnub.AddListener(_listener);
            return _pubnub;
        }

        public async void SendMsg()
        {
            (bool, MessageID) msg = await messenger.SendMsg(message, Channels.MainChannel);

            if (msg.Item1 == false)
                return;
            else
                lastMsg = msg.Item2;
        }

        public async void DeleteMsg()
        {
            long start = lastMsg.Timetoken + 1;
            long end = lastMsg.Timetoken;

            await _pubnub.DeleteMessages()
                .Channel(Channels.MainChannel)
                .Start(start)
                .End(end)
                .ExecuteAsync();

            Debug.Log($"Should delete msg: {end}");
        }

        private void OnPnMessageAction(Pubnub pn, PNMessageActionEventResult result)
        {
            Debug.Log(result.Channel);
        }

        private void OnPnSignal(Pubnub pn, PNSignalResult<object> result)
        {
            Debug.Log(result.Channel);
        }

        private void OnPnObject(Pubnub pn, PNObjectEventResult result)
        {
            Debug.Log(result.Channel);
        }

        private void OnPnFile(Pubnub pn, PNFileEventResult result)
        {
            Debug.Log(result.Channel);
        }

        private void OnPnPresence(Pubnub pn, PNPresenceEventResult result)
        {
            Debug.Log(result.Event);
        }

        private void OnPnStatus(Pubnub pn, PNStatus status)
        {
            Debug.Log(status.Category == PNStatusCategory.PNConnectedCategory ? "Connected" : "Not connected");
        }

        private void OnPnMessage(Pubnub pn, PNMessageResult<object> result)
        {
            Debug.Log($"Message received: {result.Message} | {result.Publisher} | {result.Timetoken}");
        }
        private void OnDestroy()
        {
            _pubnub.UnsibscribeAll();
        }
    }
}
