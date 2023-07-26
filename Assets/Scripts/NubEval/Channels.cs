using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NubEval
{
    public static class Channels
    {
        public static Channel Connection => new Channel("connection", ChannelType.CommChannel);
        public static Channel DebugChannel => new Channel("Channel-Barcelona", ChannelType.PresenceChannel);
        public static Channel Lobby => new Channel("lobby", ChannelType.PresenceChannel);
        public static Channel MatchesAnnouncements => new Channel("matchAnnouncements", ChannelType.CommChannel);        

        public static Channel GetMatchChannel(int matchId)
        {
            return new Channel($"match-{matchId}", ChannelType.PresenceChannel);
        }
    }
}
