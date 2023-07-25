using NubEval.Networking;

namespace NubEval
{
    [System.Serializable]
    public struct Match : INetworkDataObject
    {
        public long MatchID;
        public MatchStatus MatchStatus;
    }

    public enum MatchStatus
    {
        notStarted,
        inProgress,
        finished,
    }



}
