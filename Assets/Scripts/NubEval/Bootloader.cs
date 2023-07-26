using NubEval.Game.Networking;
using PubnubApi.Unity;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace NubEval
{
    /// <summary>
    /// 
    /// </summary>
    public class Bootloader : MonoBehaviour
    {
        [SerializeField] private PNConfigDataAsset configAsset;

        [Header("Services")]
        [SerializeField] private PlayerPrefsAsset DevAPlayerPrefs;
        [SerializeField] private UserManagementInput addUserUI;
        [SerializeField] private LobbyController lobby;
        [SerializeField] private AppDashboard dashboard;

        private PNDevice _mainDevice;
        private SubscribeCallbackListener _pnGlobalListener;

        private void Awake()
        {
            _pnGlobalListener = new SubscribeCallbackListener();

            _mainDevice = new PNDevice(configAsset.Data, DevAPlayerPrefs.PnUserID, DevAPlayerPrefs.DeviceData);
            _mainDevice.SetListener(_pnGlobalListener);

            dashboard.Construct(_mainDevice);

            lobby.Construct(_mainDevice);
            lobby.OnBoot();
            Debug.Log("Boot Complete!");
        }


        public async void KillDevice()
        {
            await _mainDevice.Disconnect();
        }

        private void OnDestroy()
        {
            _mainDevice?.Dispose();
        }

        private void OnApplicationFocus(bool focus)
        {
            Debug.LogWarning($"AppFocus: {focus}");
        }
    }
}