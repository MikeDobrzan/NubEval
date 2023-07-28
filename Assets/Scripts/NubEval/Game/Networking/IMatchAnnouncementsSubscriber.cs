using NubEval.Game.Data;
using NubEval.Game.Networking.Payload;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NubEval
{
    public interface IMatchAnnouncementsSubscriber
    {
        void OnMatchAnnounced(MatchRoomAnnouncement announcementData);
        void OnMatchroomStateUpdate(MatchRoomStatus state); 
    }
}
