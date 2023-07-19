using NubEval.Game.Networking.Data;
using NubEval.Networking;
using PubnubApi;
using System.Threading.Tasks;

namespace NubEval.Networking.PubNubWrapper
{
    public class PNPublish
    {
        private Pubnub _pubnub;

        public PNPublish(Pubnub pn)
        {
            _pubnub = pn;
        }

        public async Task<(bool, MessageID)> SendMsg(string msg, string channel)
        {
            var response = await _pubnub.Publish().Channel(channel).Message(msg).ExecuteAsync();
            var timeToken = response.Result.Timetoken;

            if (response.Status != null || response.Status.Error)
                return (false, default);
            else
                return (true, new MessageID(channel, timeToken));
        }

        public async Task<(bool, MessageID)> SendMsg(INetworkDataObject obj, string channel)
        {
            var response = await _pubnub.Publish().Channel(channel).Message(obj).ExecuteAsync();
            var timeToken = response.Result.Timetoken;

            if (response.Status != null || response.Status.Error)
                return (false, default);
            else
                return (true, new MessageID(channel, timeToken));
        }
    }
}
