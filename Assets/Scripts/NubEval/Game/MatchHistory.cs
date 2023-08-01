using NubEval.Game;
using NubEval.PubNubWrapper;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace NubEval
{
    public class MatchHistory : MonoBehaviour
    {
        [SerializeField] private string debugChannelAddress;

        [Range(0, 24)]
        [SerializeField] private int HistroyPointer;
        [SerializeField] private bool historyMode;
        [SerializeField] private int recordsCount;

        private PNDevice _device;
        private Channel _channel;
        MatchController _matchController;

        private void Awake()
        {
            States = new List<MatchStateData>();
        }

        private void Update()
        {
            recordsCount = States.Count;

            if (historyMode)
            {
                if (HistroyPointer > States.Count - 1)
                    return;

                _matchController.UpdateAvatars(States[HistroyPointer]);
            }            
        }

        public List<MatchStateData> States { get; private set; }

        public void Construct(PNDevice device, MatchController matchController)
        {
            _device = device;
            _matchController = matchController;
        }

        public void SetChannel(Channel channel)
        {
            _channel = channel;
            debugChannelAddress = channel.PubNubAddress;
        }

        public async void OnBtnGetHistory()
        {
            var messages = await _device.MessageDispatcher.HistoryGetLast25<MatchStateData>(_channel);
            States = messages;
        }
    }
}
