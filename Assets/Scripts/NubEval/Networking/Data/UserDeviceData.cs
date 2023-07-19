using UnityEngine;

namespace NubEval
{
    [System.Serializable]
    public struct UserDeviceData
    {
        [SerializeField] private string deviceID;
        [SerializeField] private DeviceType deviceType;

        public UserDeviceData(string deviceID, DeviceType deviceType)
        {
            this.deviceID = deviceID;
            this.deviceType = deviceType;
        }

        public string DeviceID { get => deviceID; }
        public DeviceType DeviceType { get => deviceType; }

        public override string ToString()
        {
            return $"(dev_{deviceID}_{deviceType})";
        }
    }

    public enum DeviceType
    {
        Unknown,
        Mobile,
        PC
    }
}
