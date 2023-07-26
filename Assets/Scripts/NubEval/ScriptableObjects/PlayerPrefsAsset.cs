using UnityEngine;

namespace NubEval
{
    [CreateAssetMenu(fileName = "debug-PlayerPefs-", menuName = "Debug/Mocks/PlayerPrefs")]
    public class PlayerPrefsAsset : ScriptableObject
    {
        [SerializeField] private string pnUserID;
        [SerializeField] private UserDeviceData deviceData;

        public string PnUserID { get => pnUserID; }
        public UserDeviceData DeviceData { get => deviceData; }
    }
}
