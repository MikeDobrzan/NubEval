using PubnubApi;
using System.Collections.Generic;

namespace NubEval
{
    public interface ILobbyEventsSubscriber
    {
        void OnUserJoin(UserId user, UserAccountData accountData, List<PresenceState> states);
        void OnUserLeave(UserId user, UserAccountData accountData);
        void OnUserChangeState(UserId user, UserAccountData accountData, List<PresenceState> states);
    }
}
