using PubnubApi;
using System.Collections.Generic;

namespace NubEval
{
    public interface ILobbyEventsHandler
    {
        void OnUserJoin(UserId user, UserAccountData accountData, List<PresenceState> states);
        void OnUserLeave(UserId user, UserAccountData accountData);
        void OnUserChangeState(UserId user, List<PresenceState> states);
    }
}
