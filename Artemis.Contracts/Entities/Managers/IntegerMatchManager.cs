using Artemis.Contracts.Entities.Interfaces;
using System.Runtime.CompilerServices;

namespace Artemis.Contracts.Entities.Managers
{
    public class IntegerMatchManager : MatchManager
    {
        private static readonly Lazy<IntegerMatchManager> Lazy =
            new(() => new());

        public static IntegerMatchManager Instance => Lazy.Value;

        public override ITuple GetSeriesResults(IMatch match, int index)
        {
            List<Shot> shots = match.GetShotsOfSeries(index);
            return new Tuple<int, double, int>(
                IntegerSeries(shots),
                DecimalSeries(shots),
                match.GetBullseyeCountOfShots(shots));
        }

        private IntegerMatchManager()
        {
        }
    }
}
