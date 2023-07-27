using PubnubApi;
using System.Collections.Generic;

namespace NubEval.Game.Networking
{
    /// <summary>
    /// Networked Player
    /// </summary>
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
