namespace NubEval.Game.Networking
{
    public interface IRemoteLobbyEventsListener
    {
        void SubscribeToLobbyEvents(ILobbyEventsSubscriber subscriber);
    }
}
