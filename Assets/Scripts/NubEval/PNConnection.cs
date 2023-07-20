using PubnubApi;
using PubnubApi.Unity;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace NubEval.Networking.PubNubWrapper
{
    public class PNConnection
    {
        private readonly UserId _userId;
        private readonly PNConfigData _config;
        private bool _connected = false;
        private Pubnub _pubnub;

        private bool _initialized;

        public PNConnection(UserId userId, PNConfigData config, SubscribeCallbackListener listener, out Pubnub pubnubApi)
        {
            if (string.IsNullOrEmpty(userId))
                Debug.LogError("Proivide userID!");

            _userId = userId;
            _config = config;

            pubnubApi = InitializePubnubApi(listener);
        }

        //TODO: temp property, remove it when refactor app dashboard
        public Pubnub PN => _pubnub;

        private Pubnub InitializePubnubApi(SubscribeCallbackListener listener)
        {
            if (_initialized)
            {
                Debug.LogError("Already Initialized! This should be called only once");
                return null;
            }

            UserId user = new UserId(_userId);
            PNConfiguration pnConfiguration = new PNConfiguration(user);
            pnConfiguration.SubscribeKey = _config.SubscribeKey;
            pnConfiguration.PublishKey = _config.PublishKey;
            pnConfiguration.UserId = _userId;

            _pubnub = new Pubnub(pnConfiguration);
            _pubnub.AddListener(listener);

            _initialized = true;

            return _pubnub;
        }

        public async Task<bool> Connect(CancellationToken token)
        {
            SubscribeCallbackListener connectionListener = new SubscribeCallbackListener();

            _pubnub.AddListener(connectionListener);
            connectionListener.onStatus += OnPnStatus;


            if (_connected) { return true; }

            _pubnub.Subscribe<string>()
                .Channels(new[] { Channels.Connection.PubNubAddress })
                .WithPresence()
                .Execute();

            await Task.Delay(1000);

            while (!_connected)
            {
                try
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(100);
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"PN Connect timed out: {e}");
                    _pubnub.RemoveListener(connectionListener);
                    return false;
                }
            }

            _pubnub.RemoveListener(connectionListener);

            return true;
        }

        public void Disconnect()
        {
            _pubnub.UnsibscribeAll();
        }

        private void OnPnStatus(Pubnub pn, PNStatus status)
        {
            if (status.Category != PNStatusCategory.PNConnectedCategory) { }
            else if (Channels.Connection.AddressMatch(status.AffectedChannels[0]))
            {
                _connected = true;
            }
            else
            {
                _connected = false;
                Debug.LogWarning("PN Connect failed");
            }
        }
    }
}
