using PubnubApi;
using System;
using System.Collections.Generic;
using UnityEngine;
using NubEval.PubNubWrapper;

namespace NubEval.Game.Data
{
    [System.Serializable]
    public struct MatchRoomStatus : INetworkDataObject
    {
        [SerializeField] private MatchProgress progress;
        [SerializeField] private List<UserId> players;
        [SerializeField] private UserId host; //the initial host it may be changed at any time
        private DateTime MatchStart;

        public MatchProgress Progress { get => progress; set => progress = value; }
    }

    public enum MatchProgress
    {
        unknown = 0,
        announced = 1,
        inProgress = 2,
        ended = 3,
        cancelled = 4
    }
}
