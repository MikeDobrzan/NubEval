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

        public List<MatchStateData> history;

        public event Action<MatchStateData> MatchStateUpdated;

        public void Construct(PNDevice device)
        {
            _device = device;
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


        public async void MockStateUpdate(MatchStateData state)
        {
            await Task.Delay(1000);

            history.Add(state);
            historyRecordCount = history.Count;

            await _device.MessageDispatcher.SendMsg(state, Channels.DebugChannel);
            Debug.Log(MatchStateData.MatchStateJSON(state));
            MatchStateUpdated?.Invoke(state);
        }

        public async void OnBtnMockStateUpdate()
        {
            //MatchStateUpdated?.Invoke(history[historyPointer]);
            await _device.MessageDispatcher.SendMsg(history[0], Channels.DebugMatchStates);
        }

        public async void OnBtnSendParticiapntData()
        {
            //MatchStateUpdated?.Invoke(history[historyPointer]);
            await _device.MessageDispatcher.SendMsg(participantData, Channels.DebugMatchStates);
        }
    }
}
