using Newtonsoft.Json;
using NubEval.Game.Networking;
using PubnubApi;
using System.Collections.Generic;
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

                string json = JsonConvert.SerializeObject(obj);

                //Debug.Log($"msg({id.Channel},{id.Timetoken}) : SENT: {obj}");
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

        public async Task<List<T>> HistoryGetLast25<T>(Channel channel)
        {
            var response = await PNApi.FetchHistory()
                .Channels(new string[] { channel.PubNubAddress })
                .IncludeMeta(false)
                .MaximumPerChannel(25)
                .ExecuteAsync();

            Dictionary<string, List<PNHistoryItemResult>> resultDict = response.Result.Messages;

            List<T> msgList = new List<T>();

            foreach (var resultList in resultDict.Values)
            {
                foreach (var obj in resultList)
                {
                    string str = JsonConvert.SerializeObject(obj.Entry);
                    msgList.Add(JsonConvert.DeserializeObject<T>(str));
                    Debug.Log(JsonConvert.SerializeObject(obj.Entry));
                }
            }

            return msgList;
        }
    }
}
