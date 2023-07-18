using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NubEval
{
    [System.Serializable]
    public struct PresenceState
    {
        public string StateType;
        public string State;

        public PresenceState(string stateType, string state)
        {
            StateType = stateType;
            State = state;
        }
    }
}
