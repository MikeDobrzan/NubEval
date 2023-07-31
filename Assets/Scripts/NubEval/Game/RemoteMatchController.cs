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


        public List<MatchStateData> history;

        public event Action<MatchStateData> MatchStateUpdated;

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

            Debug.Log(MatchStateData.MatchStateJSON(state));
            MatchStateUpdated?.Invoke(state);
        }

        public void OnBtnMockStateUpdate()
        {
            MatchStateUpdated?.Invoke(history[historyPointer]);
        }
    }
}
