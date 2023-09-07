using Artemis.Contracts.DTOs;
using Artemis.Contracts.Entities.Interfaces;
using Artemis.Contracts.Entities.Managers;

namespace Artemis.Contracts.Entities.Matches
{
    public class AR10Match : BullseyeMatch
    {
        protected override IMatchManager Manager => DecimalMatchManager.Instance;

        public AR10Match() : base()
        {
        }

        public AR10Match(
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

        public AR10Match(
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

        public AR10Match(MatchCreateRequestDto createRequest)
            : base(createRequest)
        {
        }

        public AR10Match(MatchUpdateRequestDto matchUpdateRequest)
            : base(matchUpdateRequest)
        {
        }

        public AR10Match(MatchOutputDto matchOutput)
            : base(matchOutput)
        {
        }
    }
}
