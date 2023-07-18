using PubnubApi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NubEval
{
    public class ConnectedUser
    {
        public UserId ID;
        public List<PresenceState> PresenceStates;

        public ConnectedUser(UserId iD, List<PresenceState> presenceStates)
        {
            ID = iD;
            PresenceStates = presenceStates;
        }
    }
}
