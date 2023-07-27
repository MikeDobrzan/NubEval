using NubEval.DevTools;
using NubEval.Game.Networking;
using NubEval.PubNubWrapper;
using NubEval.PubNubWrapper.ScriptableObjects;
using PubnubApi.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace NubEval.Game
{
    /// <summary>
    /// 
    /// </summary>
    public class Bootloader : MonoBehaviour
    {
        [SerializeField] private PNConfigDataAsset configAsset;

        [Header("Services")]
        [SerializeField] private PlayerPrefsAsset DevAPlayerPrefs;
        [SerializeField] private LobbyController lobby;

        private PNDevice _mainDevice;
        private SubscribeCallbackListener _pnGlobalListener;

        public PNDevice PNDevice => _mainDevice;

        public event Action<PNDevice> OnPubNubDeviceInitialized;

        private void Awake()
        {
            //Network Boot Actions
            InitializePubNub();
            SubscribeToSystemChannels();

            //Controllers Boot Actions
            InitializeLobby();

            Debug.Log("Boot Complete!");
        }

        public async void SubscribeToSystemChannels()
        {
            List<Channel> channels = new List<Channel>
            {
                Channels.Lobby,
                Channels.MatchesAnnouncements
            };

            await _mainDevice.Subscriptions.SubscribeChannels(channels);
        }

        public void InitializePubNub()
        {
            _pnGlobalListener = new SubscribeCallbackListener();
            _mainDevice = new PNDevice(configAsset.Data, DevAPlayerPrefs.PnUserID, DevAPlayerPrefs.DeviceData);
            _mainDevice.SetListener(_pnGlobalListener);
            OnPubNubDeviceInitialized?.Invoke(_mainDevice);
        }

        public void InitializeLobby()
        {
            lobby.Construct(_mainDevice);
            lobby.OnBoot();
        }

        public async void PNJoinLobby()
        {
            await _mainDevice.Presence.SubscribePresence(Channels.Lobby);
        }

        private void OnDestroy()
        {
            _mainDevice?.Dispose();
        }

        private void OnApplicationFocus(bool focus)
        {
            //Debug.LogWarning($"AppFocus: {focus}");
        }
    }
}