using Artemis.Contracts.DTOs;
using Artemis.Contracts.Entities.Interfaces;
using Artemis.Contracts.Entities.Managers;

namespace Artemis.Contracts.Entities.Matches
{
    public class _3P50Match : PhasedBullseyeMatch
    {
        protected override IMatchManager Manager => DecimalMatchManager.Instance;

        protected override int PhasesInMatch => 3;

        protected override int SeriesInPhase => 4;

        public _3P50Match() : base()
        {
        }

        public _3P50Match(
            User shooter,
            Timestamp startTimestamp,
            Timestamp endTimestamp,
            Location location,
            double? airTemperature = null,
            double? airPressure = null,
            double? windSpeed = null,
            string? windDirection = null,
            string? environmentNotes = null,
            string? equipmentNotes = null,
            string? shooterNotes = null)
            : base(
                shooter,
                startTimestamp,
                endTimestamp,
                location,
                airTemperature,
                airPressure,
                windSpeed,
                windDirection,
                environmentNotes,
                equipmentNotes,
                shooterNotes)
        {
        }

        public _3P50Match(
            string id,
            User shooter,
            Timestamp startTimestamp,
            Timestamp endTimestamp,
            Location location,
            List<Shot> shots,
            double? airTemperature = null,
            double? airPressure = null,
            double? windSpeed = null,
            string? windDirection = null,
            string? environmentNotes = null,
            string? equipmentNotes = null,
            string? shooterNotes = null)
            : base(
                id,
                shooter,
                startTimestamp,
                endTimestamp,
                location,
                shots,
                airTemperature,
                airPressure,
                windSpeed,
                windDirection,
                environmentNotes,
                equipmentNotes,
                shooterNotes)
        {
        }

        public _3P50Match(MatchCreateRequestDto createRequest)
            : base(createRequest)
        {
        }

        public _3P50Match(MatchUpdateRequestDto matchUpdateRequest)
            : base(matchUpdateRequest)
        {
        }

        public _3P50Match(MatchOutputDto matchOutput)
            : base(matchOutput)
        {
        }
    }
}
