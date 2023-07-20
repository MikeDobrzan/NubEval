using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NubEval.Networking
{
    public interface IRemoteLobbyEventsListener
    {
        void SubscribeLobbyEvents(ILobbyEventsHandler subscriber);
    }
}
