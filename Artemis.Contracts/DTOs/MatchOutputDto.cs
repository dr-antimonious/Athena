using Artemis.Contracts.Entities;
using Artemis.Contracts.Entities.Matches;

namespace Artemis.Contracts.DTOs
{
    public class MatchOutputDto : MatchUpdateRequestDto
    {
        public UserDto Shooter { get; set; }

        public Timestamp StartTimestamp { get; set; }

        public Timestamp EndTimestamp { get; set; }

        public Location Location { get; set; }

        public MatchOutputDto() : base()
        {
        }

        public MatchOutputDto(
            string id,
            string type,
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
                type,
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

        public MatchOutputDto(Match match)
            : base(match)
        {
            Shooter = match.Shooter.CreateDto();
            StartTimestamp = match.StartTimestamp;
            EndTimestamp = match.EndTimestamp;
            Location = match.Location;
        }
    }
}
