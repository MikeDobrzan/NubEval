using Newtonsoft.Json;
using NubEval.PubNubWrapper;
using PubnubApi;
using UnityEngine;

namespace NubEval
{
    [System.Serializable]
    public struct ParticipantData : INetworkDataObject
    {
        [SerializeField] private int index;
        [SerializeField] private string pnUser;

        public ParticipantData(int index, UserId user )
        {
            this.index = index;
            this.pnUser = user;
        }

        [JsonProperty] public int Index { get => index; set => index = value; }
        [JsonProperty("uuid")] public string PnUser { get => pnUser; set => pnUser = value; }
    }
}
