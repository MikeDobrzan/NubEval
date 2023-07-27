using PubnubApi;
using System.Threading;
using System.Threading.Tasks;

namespace NubEval
{
    public interface IPresenceEventHandler
    {
        Task OnEventAsync(PNPresenceEventResult eventResult, CancellationToken token);
    }
}
