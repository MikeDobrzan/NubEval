using PubnubApi;
using System.Threading;
using System.Threading.Tasks;

namespace NubEval.PubNubWrapper
{
    public interface IMessageEventHandler
    {
        Task OnEventAsync(PNMessageResult<object> eventResult, CancellationToken token);
    }
}
