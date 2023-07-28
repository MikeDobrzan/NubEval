using Newtonsoft.Json;
using NubEval.Game.Networking;
using PubnubApi;
using System.Threading.Tasks;
using UnityEngine;

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

            if (response.Result == null || response.Status.Error)
            {
                Debug.Log($"msg : ERROR");
                return (false, default);
            }
            else
            {
                var id = new MessageID(channel.PubNubAddress, response.Result.Timetoken);

                Debug.Log($"msg({id.Channel},{id.Timetoken}) : SENT");
                return (true, id);
            }
        }

        public async Task DeleteFromHistory(MessageID message)
        {
            var response = await PNApi.DeleteMessages()
                .Channel(message.Channel)
                .Start(message.DelStart)
                .End(message.DelEnd)
                .ExecuteAsync();

            Debug.Log($"[DeleteFromHistory]{JsonConvert.SerializeObject(response)}");
        }
    }
}
