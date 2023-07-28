using Newtonsoft.Json;
using PubnubApi;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace NubEval.PubNubWrapper
{
    public class PNDatastoreChannels : PNServiceBase
    {
        private const string DEFAULT_KEY = "default";

        public PNDatastoreChannels(Pubnub pubnub, PNDevice device) : base(pubnub, device)
        {
        }

        public async Task<T> SetDefaultCustomData<T>(Channel channel, T data)
        {
            string dataJSON = JsonConvert.SerializeObject(data);

            var dict = new Dictionary<string, object>
            {
                {DEFAULT_KEY, dataJSON }
            };

            var response = await PNApi.SetChannelMetadata()
                .Channel(channel.PubNubAddress)
                .Name("default-name")
                .Custom(dict)
                .IncludeCustom(true)
                .ExecuteAsync();

            response.Result.Custom.TryGetValue(DEFAULT_KEY, out object obj);
            string returnJSON = (string)obj;

            var returnData = JsonConvert.DeserializeObject<T>(returnJSON);
            return returnData;
        }

        public async Task<T> GetDefaultCustomData<T>(Channel channel)
        {
            T data = default;

            var response = await PNApi.GetChannelMetadata()
                .Channel(channel.PubNubAddress)
                .IncludeCustom(true)
                .ExecuteAsync();

            if (response != null)
            {
                if (response.Result != null)
                {
                    if (response.Result.Custom != null)
                    {
                        response.Result.Custom.TryGetValue(DEFAULT_KEY, out object obj);

                        string dataJSON = (string)obj;

                        data = JsonConvert.DeserializeObject<T>(dataJSON);
                    }
                    else
                        Debug.LogWarning("Data Error");
                }
            }
            return data;
        }
    }
}
