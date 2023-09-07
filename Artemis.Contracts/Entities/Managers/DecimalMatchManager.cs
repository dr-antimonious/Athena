using Artemis.Contracts.Entities.Interfaces;
using System.Runtime.CompilerServices;

namespace Artemis.Contracts.Entities.Managers
{
    public class DecimalMatchManager : MatchManager
    {
        private static readonly Lazy<DecimalMatchManager> Lazy =
            new(() => new());

        public static DecimalMatchManager Instance => Lazy.Value;

        public override ITuple GetSeriesResults(IMatch match, int index)
        {
            List<Shot> shots = match.GetShotsOfSeries(index);
            return new Tuple<int?, double, int>(
                null,
                DecimalSeries(shots),
                match.GetBullseyeCountOfShots(shots));
        }

        private DecimalMatchManager()
        {
        }
    }
}
