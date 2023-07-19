using NubEval.Game.Networking.Data;
using PubnubApi;
using System.Threading.Tasks;

namespace NubEval.Networking.PubNubWrapper
{
    public class PNPublish : PNServiceBase
    {
        public PNPublish(Pubnub pubnub) : base(pubnub) { }

        public async Task<(bool, MessageID)> SendMsg(string msg, string channel)
        {
            var response = await Pubnub.Publish().Channel(channel).Message(msg).ExecuteAsync();
            var timeToken = response.Result.Timetoken;

            if (response.Status != null || response.Status.Error)
                return (false, default);
            else
                return (true, new MessageID(channel, timeToken));
        }

        public async Task<(bool, MessageID)> SendMsg(INetworkDataObject obj, string channel)
        {
            var response = await Pubnub.Publish().Channel(channel).Message(obj).ExecuteAsync();
            var timeToken = response.Result.Timetoken;

            if (response.Status != null || response.Status.Error)
                return (false, default);
            else
                return (true, new MessageID(channel, timeToken));
        }
    }
}
