using Artemis.Contracts.DTOs;
using Artemis.Contracts.Entities.Interfaces;

namespace Artemis.Contracts.Entities.Matches
{
    public class TSMatch : Match
    {
        protected override int ShotsInSeries => 25;

        protected override int SeriesInPhase => 5;

        public TSMatch() : base()
        {
        }

        public TSMatch(
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

        public TSMatch(
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

        public TSMatch(MatchCreateRequestDto createRequest)
            : base(createRequest)
        {
        }

        public TSMatch(MatchUpdateRequestDto matchUpdateRequest)
            : base(matchUpdateRequest)
        {
        }

        public TSMatch(MatchOutputDto matchOutput)
            : base(matchOutput)
        {
        }
    }
}
