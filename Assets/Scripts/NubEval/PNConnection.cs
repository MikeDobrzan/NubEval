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

        public PNConnection(UserAccountData account, PNConfigData config)
        {
            UserId userId = account.PubNubUserID;

            if (string.IsNullOrEmpty(userId))
            {
                Debug.LogError("Proivide userID!");
                // It is recommended to change the UserId to a meaningful value, to be able to identify this client.
                //userId = System.Guid.NewGuid().ToString();
            }

            _userId = userId;
            _config = config;
        }

        public UserId UserID => _userId;

        //public async Task<Pubnub> Connect()
        //{

        //}

        public async Task<Pubnub> SetListener(SubscribeCallbackListener listener, CancellationToken token)
        {
            SubscribeCallbackListener connectionListener = new SubscribeCallbackListener();

            _pubnub = Initialize(_userId);
            _pubnub.AddListener(connectionListener);
            _pubnub.AddListener(listener);            
            connectionListener.onStatus += OnPnStatus;

            _pubnub.Subscribe<string>().Channels(new[] { "boot-connect" }).Execute();

            while (!_connected)
            {
                try
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(100);
                }
                catch (System.Exception e)
                {
                    Debug.LogError("PN Connect timed out");
                    _pubnub.RemoveListener(connectionListener);
                    throw e;
                }
            }

            _pubnub.RemoveListener(connectionListener);
            return _pubnub;
        }

        private Pubnub Initialize(string userId)
        {
            UserId user = new UserId(userId);
            PNConfiguration pnConfiguration = new PNConfiguration(user);
            pnConfiguration.SubscribeKey = _config.SubscribeKey;
            pnConfiguration.PublishKey = _config.PublishKey;
            pnConfiguration.UserId = userId;

            var pubnub = new Pubnub(pnConfiguration);
            return pubnub;
        }

        public void OnPnStatus(Pubnub pn, PNStatus status)
        {           
            if (status.Category == PNStatusCategory.PNConnectedCategory)
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
