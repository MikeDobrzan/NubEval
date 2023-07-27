namespace NubEval.Game.Networking
{
    [System.Serializable]
    public struct MessageID
    {
        public string Channel;
        public long Timetoken;

        public MessageID(string channel, long timetoken)
        {
            Channel = channel;
            Timetoken = timetoken;
        }
    }
}
