using Newtonsoft.Json;
using NubEval.Game.Networking;
using NubEval.PubNubWrapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace NubEval
{
    public class RemoteMatchController : MonoBehaviour
    {
        [SerializeField] private List<ParticipantData> mock_participants;
        [SerializeField] private int historyPointer;
        [SerializeField] private int historyRecordCount;
        [SerializeField] private ParticipantData participantData;

        private PNDevice _device;

        private List<MatchStateData> _history;
        public List<MatchStateData> History => _history;

        private Channel _channel;

        public void Construct(PNDevice device)
        {
            _device = device;
            _history = new List<MatchStateData>();
        }

        public void SetChannel(Channel channel)
        {
            _channel = channel;
        }

        public List<ParticipantData> GetParticipants()
        {
            var participantDatas = new List<ParticipantData>();
            foreach (var p in mock_participants)
            {
                participantDatas.Add(p);
            }

            return participantDatas;
        }

        public async void PublishStateUpdate(MatchStateData state)
        {
            await _device.MessageDispatcher.SendMsg(state, _channel);
            //Debug.Log(MatchStateData.MatchStateJSON(state));
            _history.Add(state);
            historyRecordCount = _history.Count;
        }

        public async void OnBtnMockStateUpdate()
        {
            await _device.MessageDispatcher.SendMsg(_history[historyPointer], Channels.DebugMatchStates);
        }

        public async void OnBtnSendParticiapntData()
        {
            await _device.MessageDispatcher.SendMsg(participantData, Channels.DebugMatchStates);
        }
    }
}
