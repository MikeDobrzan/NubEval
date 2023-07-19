namespace NubEval
{
    public struct Channel
    {
        private string _pnId;
        private ChannelType _channelType;

        public Channel(string pnId, ChannelType channelType)
        {
            _pnId = pnId;
            _channelType = channelType;
        }

        public string PubNubAddress => _pnId;
        public ChannelType ChannelType => _channelType;
    }

    public enum ChannelType
    {
        PublishOnlyChannel,
        PresenceChannel
    }
}
