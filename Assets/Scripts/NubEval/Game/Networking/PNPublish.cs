using NubEval.Game.Networking.Data;
using PubnubApi;
using System.Threading.Tasks;

namespace NubEval.PubNubWrapper
{
    public class PNPublish : PNServiceBase
    {
        public PNPublish(Pubnub pubnub, PNDevice device) : base(pubnub, device) { }

        public async Task<(bool, MessageID)> SendMsg(string msg, Channel channel)
        {
            var response = await PNApi.Publish().Channel(channel.PubNubAddress).Message(msg).ExecuteAsync();
            var timeToken = response.Result.Timetoken;

            if (response.Status != null || response.Status.Error)
                return (false, default);
            else
                return (true, new MessageID(channel.PubNubAddress, timeToken));
        }

        public async Task<(bool, MessageID)> SendMsg(INetworkDataObject obj, Channel channel)
        {
            var response = await PNApi.Publish().Channel(channel.PubNubAddress).Message(obj).ExecuteAsync();
            var timeToken = response.Result.Timetoken;

            if (response.Status != null || response.Status.Error)
                return (false, default);
            else
                return (true, new MessageID(channel.PubNubAddress, timeToken));
        }
    }
}
