using NubEval.Networking;
using PubnubApi;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;
using Codice.Client.BaseCommands;

namespace NubEval
{
    public class PNDatastoreChannels : PNServiceBase
    {
        private const string DEFAULT_KEY = "default";


        public PNDatastoreChannels(Pubnub pubnub, PNDevice device) : base(pubnub, device)
        {
        }

        //public async Task SetDefaultCustomData(Channel channel, string data)
        //{
        //    var dict = new Dictionary<string, object>
        //    {
        //        {DEFAULT_KEY, data }
        //    };

        //    var response = await PNApi.SetChannelMetadata()
        //        .Channel(channel.PubNubAddress)
        //        .Name("default-name")
        //        .Custom(dict)
        //        .IncludeCustom(true)
        //        .ExecuteAsync();

        //    Debug.Log($"SET result={response.Result != null}");
        //}


        public async Task SetDefaultCustomData<T>(Channel channel, T data)
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

            
            Debug.Log($"SET status= {response != null} | result={response.Result != null}");
        }

        public async Task<T> GetDefaultCustomData<T>(Channel channel)
        {
            string debug = "[PNDatastoreChannels] ";

            T data = default;

            var response = await PNApi.GetChannelMetadata()
                .Channel(channel.PubNubAddress)
                .IncludeCustom(true)
                .ExecuteAsync();

            debug += $"response={response != null} |";
            if (response != null)
            {
                debug += $"result={response.Result != null} |";
                if (response.Result != null)
                {
                    debug += $"Custom={response.Result.Custom != null} |";
                    if (response.Result.Custom != null)
                    {
                        response.Result.Custom.TryGetValue(DEFAULT_KEY, out object obj);

                        string dataJSON = (string)obj;

                        Debug.Log($"RAW={dataJSON}");

                        data = JsonConvert.DeserializeObject<T>(dataJSON);
                    }
                    else
                        Debug.LogWarning("Data Error");
                }
            }

            Debug.Log(debug);

            return data;
        }
    }
}
