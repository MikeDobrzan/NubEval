using PubnubApi;
using PubnubApi.Unity;
using UnityEngine;
using static PubnubApi.Unity.PubnubExtensions;

namespace NubEval.Networking.PubNubWrapper
{
    public class PNConnection
    {
        private readonly UserId _userId;
        private readonly PNConfigAsset _config;
        //private Pubnub _pubnub;

        public PNConnection(UserId userId, PNConfigAsset config)
        {
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

        public Pubnub Connect(SubscribeCallbackListener listener)
        {
            var pubnub = Initialize(_userId);
            pubnub.AddListener(listener);

            return pubnub;
        }

        private Pubnub Initialize(string userId)
        {
            UserId user = new UserId(userId);
            PNConfiguration pnConfiguration = new PNConfiguration(user);
            pnConfiguration.SubscribeKey = _config.SubscribeKey;
            pnConfiguration.PublishKey = _config.PublishKey;

            //if (_pubnub is not null)
            //{
            //    Debug.LogError("PubNub has already been initialized");
            //    return _pubnub;
            //}

            pnConfiguration.UserId = userId;
            var pubnub = new Pubnub(pnConfiguration);
            return pubnub;
        }
    }
}
