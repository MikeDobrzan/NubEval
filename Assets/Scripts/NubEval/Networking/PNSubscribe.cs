using PubnubApi;
using PubnubApi.Unity;
using System.Collections.Generic;

namespace NubEval
{
    public class PNSubscribe : PNServiceBase
    {
        public PNSubscribe(Pubnub pubnub, PNDevice device) : base(pubnub, device)
        {

        }

        public void SubscribeChannels(List<Channel> channels)
        {
            if (channels == null || channels.Count == 0) return;

            List<string> publishChannels = new List<string>();
            List<string> presenceChannels = new List<string>();

            foreach (var ch in channels)
            {
                if (ch.ChannelType == ChannelType.PublishOnlyChannel)
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

            if (presenceChannels.Count > 0)
            {
                PNApi.Subscribe<string>()
                    .Channels(presenceChannels)
                    .WithPresence()
                    .Execute();
            }

            PNDevice.Console.Log($"[SubscribeChannels] {channels[0].PubNubAddress}");
        }
    }
}

