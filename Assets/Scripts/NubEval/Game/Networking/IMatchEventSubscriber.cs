using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace NubEval.Game.Networking
{
    public interface IMatchEventSubscriber
    {
        Task OnMatchStateUpdate(MatchStateData matchStateData);
    }
}
