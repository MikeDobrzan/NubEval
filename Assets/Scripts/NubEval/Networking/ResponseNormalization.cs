using NubEval.Game.Networking;
using System.Collections.Generic;
using UnityEngine;

namespace NubEval.Networking.PubNubWrapper
{
    public static class ResponseNormalization
    {
        private const string EVENT_JOIN = "join";
        private const string EVENT_LEAVE = "leave";
        private const string EVENT_CHANGE_STATE = "state-change";

        public static PresencelEvent ToPresenceEventType(string eventType)
        {
            if (string.Equals(eventType, EVENT_JOIN))
                return PresencelEvent.Join;

            if (string.Equals(eventType, EVENT_LEAVE))
                return PresencelEvent.Leave;

            if (string.Equals(eventType, EVENT_CHANGE_STATE))
                return PresencelEvent.ChangeState;

            return PresencelEvent.unknown;
        }

        public static bool IsValidPresenceState(Dictionary<string, object> dict)
        {
            if(dict == null)
                 return false;              

            bool hasLobbyState = dict.ContainsKey(PresenceState.LobbyStateString);
            if (!hasLobbyState)
                return false;

            return true;
        }

        //keys that do not match the defined states will be ignored
        public static List<PresenceState> ToPresenceStates(Dictionary<string, object> dict) 
        {
            List<PresenceState> states = new List<PresenceState>();

            if (dict == null)
                return states;

            if (dict.TryGetValue(PresenceState.LobbyStateString, out object lobbyStateObj))
                states.Add(new PresenceState(StateType.lobbyState, (string)lobbyStateObj));

            if (dict.TryGetValue(PresenceState.MatchStateString, out object matchState))
                states.Add(new PresenceState(StateType.matchState, (string)matchState));

            return states;
        }

        //states list should contain the nesesary data
        public static Dictionary<string, object> BuildStatesDict(List<PresenceState> states)
        {
            var dict = new Dictionary<string, object>();

            foreach (var state in states)
            {
                string key = state.StateType.ToString();
                string value = state.State.ToString();
                dict.Add(key, value);
            }

            return dict;
        }
    }
}
