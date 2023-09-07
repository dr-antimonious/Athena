using Artemis.Contracts.DTOs;
using Artemis.Contracts.Entities.Interfaces;
using Artemis.Contracts.Entities.Managers;

namespace Artemis.Contracts.Entities.Matches
{
    public class P25Match : PhasedBullseyeMatch
    {
        protected override IMatchManager Manager => IntegerMatchManager.Instance;

        public P25Match() : base()
        {
        }

        public P25Match(
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

        public P25Match(
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

        public P25Match(MatchCreateRequestDto createRequest)
            : base(createRequest)
        {
        }

        public P25Match(MatchUpdateRequestDto matchUpdateRequest)
            : base(matchUpdateRequest)
        {
        }

        public P25Match(MatchOutputDto matchOutput)
            : base(matchOutput)
        {
        }
    }
}
