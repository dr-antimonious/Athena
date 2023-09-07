using System.Runtime.CompilerServices;

namespace Artemis.Contracts.Entities.Interfaces
{
    public interface IMatchManager
    {
        ITuple GetSeriesResults(IMatch match, int index);

        ITuple GetPhaseResults(IMatch match, int index);

        ITuple GetMatchResult(IMatch match);
    }
}
