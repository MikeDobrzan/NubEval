using NubEval.Networking;
using PubnubApi;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace NubEval
{
    public class PNDatastoreChannels : PNServiceBase
    {
        private const string DEFAULT_KEY = "default";


        public PNDatastoreChannels(Pubnub pubnub, PNDevice device) : base(pubnub, device)
        {
        }

        public async Task SetDefaultCustomData(Channel channel, INetworkDataObject data)
        {

            var dict = new Dictionary<string, object>
            {
                {DEFAULT_KEY, data }
            };

            await PNApi.SetChannelMetadata()
                .Channel(channel.PubNubAddress)
                .Custom(dict)
                .IncludeCustom(true)
                .ExecuteAsync();
        }

        public async Task<T> GetDefaultCustomData<T>(Channel channel)
            where T : struct, INetworkDataObject
        {
            T data = default;

            var response = await PNApi.GetChannelMetadata()
                .Channel(channel.PubNubAddress)
                .IncludeCustom(true)
                .ExecuteAsync();

            if (response != null && response.Result != null)
            {
                if (response.Result.Custom != null)
                {
                    response.Result.Custom.TryGetValue(DEFAULT_KEY, out object obj);

                    data = (T)obj;
                }                   
                else
                    Debug.LogWarning("Data Error");
            }

            return data;
        }
    }
}
