using PubnubApi;

namespace NubEval
{
    public interface ILobbyEventsHandler
    {
        void OnUserJoin(UserId user, UserAccountData accountData);
        void OnUserLeave(UserId user, UserAccountData accountData);
    }
}
