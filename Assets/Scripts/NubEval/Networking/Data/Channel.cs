using UnityEngine;

namespace NubEval
{
    [System.Serializable]
    public struct Channel
    {
        [SerializeField] private string _pnId;
        [SerializeField] private ChannelType _channelType;

        public Channel(string pnId, ChannelType channelType)
        {
            _pnId = pnId;
            _channelType = channelType;
        }

        public string PubNubAddress => _pnId;
        public string PresenceAddress => _pnId + "-pnpres";
        public ChannelType ChannelType => _channelType;

        public bool AddressMatch(string str)
        {
            return string.Equals(_pnId, str);
        }
    }

    public enum ChannelType
    {
        PublishOnlyChannel,
        PresenceChannel
    }
}
