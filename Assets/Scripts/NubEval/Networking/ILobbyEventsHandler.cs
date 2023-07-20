using PubnubApi;

namespace NubEval
{
    public interface ILobbyEventsHandler
    {
        void OnUserJoined(UserId user, UserAccountData accountData);
    }
}
