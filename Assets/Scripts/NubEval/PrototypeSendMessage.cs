using NubEval.Game.Networking.Data;
using PubnubApi.Unity;
using System.Threading.Tasks;
using UnityEngine;

namespace NubEval
{
    public class PrototypeSendMessage : MonoBehaviour
    {
        [SerializeField] private PNDevice pn;

        [SerializeField] private string message;
        [SerializeField] private Channel channel;
        [SerializeField] private Match testMatch;
        [SerializeField] private PNConfigAsset config;
        [SerializeField] private MessageID lastMsg;

        public async void SendNetworkObject()
        {
            (bool, MessageID) msg = await pn.MessageDispatcher.SendMsg(testMatch, Channels.DebugChannel);

            if (msg.Item1 == false)
                return;
            else
                lastMsg = msg.Item2;
        }

        public async void SendMsg()
        {
            await SendMsg(message, channel);
        }

        private async Task SendMsg(string message, Channel channel)
        {
            (bool, MessageID) msg = await pn.MessageDispatcher.SendMsg(message, channel);

            if (msg.Item1 == false)
                return;
            else
                lastMsg = msg.Item2;
        }
    }
}
