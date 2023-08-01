using NubEval.Game.Networking;

namespace NubEval
{
    public interface IMatchEventsListener
    {
        void SubscribeMatchEvents(IMatchEventSubscriber subscriber);
    }
}
