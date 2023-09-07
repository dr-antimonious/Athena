using Artemis.Contracts.DTOs;

namespace Artemis.Contracts.Entities.Matches
{
    public abstract class BullseyeMatch : Match
    {
        public const double BullseyeMinimum = 10.4;

        public override int GetTotalBullseyeCount()
            => Shots.Count(x => x.Value >= BullseyeMinimum);

        public override int GetBullseyeCountOfShots(List<Shot> shots)
            => shots.Count(x => x.Value >= BullseyeMinimum);

        protected BullseyeMatch() : base()
        {
        }

        protected BullseyeMatch(
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

        protected BullseyeMatch(
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

        protected BullseyeMatch(MatchCreateRequestDto createRequest)
            : base(createRequest)
        {
        }

        protected BullseyeMatch(MatchUpdateRequestDto matchUpdateRequest)
            : base(matchUpdateRequest)
        {
        }

        protected BullseyeMatch(MatchOutputDto matchOutput)
            : base(matchOutput)
        {
        }
    }
}
