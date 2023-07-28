using System.Threading;
using System.Threading.Tasks;

namespace NubEval.Game
{
    public interface IMatchParticipant
    {
        //PlayerState PlayerState { get; }
        //void ApplyState(PlayerState state);
        Task<PlayerAction> RequestActionAsync(PlayerTurn playerTurn, CancellationToken token);
    }
}
