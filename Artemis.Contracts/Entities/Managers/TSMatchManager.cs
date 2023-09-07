using Artemis.Contracts.Entities.Interfaces;
using System.Runtime.CompilerServices;

namespace Artemis.Contracts.Entities.Managers
{
    public class TSMatchManager : MatchManager
    {
        private static readonly Lazy<TSMatchManager> Lazy =
            new(() => new());

        public static TSMatchManager Instance => Lazy.Value;

        public override ITuple GetSeriesResults(IMatch match, int index)
        {
            List<Shot> shots = match.GetShotsOfSeries(index);
            return new Tuple<int, double?, int?>(
                IntegerSeries(shots),
                null,
                null);
        }

        private TSMatchManager()
        {
        }
    }
}
