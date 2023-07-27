namespace NubEval.Game.Networking
{
    [System.Serializable]
    public struct PresenceState
    {
        public StateType StateType;
        public string State;

        public PresenceState(StateType stateType, string state)
        {
            StateType = stateType;
            State = state;
        }

        public static string LobbyStateString => "lobbyState";
        public static string MatchStateString => "matchState";
    }

    public enum StateType
    {
        lobbyState = 0,
        matchState = 1,
    }
}
