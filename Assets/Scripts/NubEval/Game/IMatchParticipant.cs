using System.Threading;
using System.Threading.Tasks;

namespace NubEval.Game
{
    public interface IMatchParticipant
    {
        //PlayerState PlayerState { get; }
        //void ApplyState(PlayerState state);
        ParticipantData ParticipantID { get; }
        PlayerData PlayerData { get; }
        Task<PlayerAction> RequestActionAsync(PlayerTurn playerTurn, CancellationToken token);
    }
}
