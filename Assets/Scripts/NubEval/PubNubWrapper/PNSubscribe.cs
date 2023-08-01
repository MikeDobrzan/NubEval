using PubnubApi;
using PubnubApi.Unity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NubEval.PubNubWrapper
{
    public class PNSubscribe : PNServiceBase
    {
        public PNSubscribe(Pubnub pubnub, PNDevice device) : base(pubnub, device)
        {

        }


        public async Task SubscribeChannels(Channel channel)
        {
            await SubscribeChannels(new List<Channel> { channel });
        }

        public async Task SubscribeChannels(List<Channel> channels)
        {
            if (channels == null || channels.Count == 0) return;

            List<string> publishChannels = new List<string>();
            List<string> presenceChannels = new List<string>();

            foreach (var ch in channels)
            {
                if (ch.ChannelType == ChannelType.CommChannel)
                    publishChannels.Add(ch.PubNubAddress);

                if (ch.ChannelType == ChannelType.PresenceChannel)
                    presenceChannels.Add(ch.PubNubAddress);
            }

            if (publishChannels.Count > 0)
            {
                PNApi.Subscribe<string>()
                    .Channels(publishChannels)
                    .Execute();
            }

            await Task.Delay(1000);

            if (presenceChannels.Count > 0)
            {
                PNApi.Subscribe<string>()
                    .Channels(presenceChannels)
                    .WithPresence()
                    .Execute();
            }

            await Task.Delay(1000);

            //PNDevice.Console.Log($"[SubscribeChannels] {channels[0].PubNubAddress}");
        }

        public async Task Subscribe<T>(Channel channel)
        {
            PNApi.Subscribe<T>()
                .Channels(new[] { channel.PubNubAddress })
                .WithPresence()
                .Execute();

            await Task.Delay(1000);
        }
    }
}

