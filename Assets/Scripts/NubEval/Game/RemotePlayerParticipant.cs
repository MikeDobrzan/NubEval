using NubEval.Game;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace NubEval
{
    public class RemotePlayerParticipant : IMatchParticipant
    {
        private PlayerData _playerData;
        private ParticipantID _participantID;

        public RemotePlayerParticipant(ParticipantID id, PlayerData playerData)
        {
            _participantID = id;
            _playerData = playerData;
        }

        PlayerData IMatchParticipant.PlayerData => _playerData;

        ParticipantID IMatchParticipant.ParticipantID => _participantID;

        public Task<PlayerAction> RequestActionAsync(PlayerTurn playerTurn, CancellationToken token)
        {
            Debug.LogError("Remote players should not create actions!!!");
            return default;
        }

        Task<PlayerAction> IMatchParticipant.RequestActionAsync(PlayerTurn playerTurn, CancellationToken token)
        {
            throw new System.NotImplementedException();
        }
    }
}
