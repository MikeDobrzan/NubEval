using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NubEval
{
    public static class Channels
    {
        public static Channel Connection = new Channel("connection", ChannelType.PublishOnlyChannel);
        public static Channel DebugChannel = new Channel("Channel-Barcelona", ChannelType.PresenceChannel);
        public static Channel Lobby = new Channel("lobby", ChannelType.PresenceChannel);
        
        //public static string Lobby = "friends";
        //public static string LobbyMatches = "app-channel-lobby";
    }
}
