using System.ComponentModel.DataAnnotations;
using Artemis.Contracts.Entities;
using Artemis.Contracts.Entities.Matches;

namespace Artemis.Contracts.DTOs
{
    public class MatchUpdateRequestDto : MatchCreateRequestDto
    {
        [Required(ErrorMessage = "Id is required")]
        public string Id { get; set; }

        [Required(ErrorMessage = "Shots are required")]
        public new List<ExtendedShotDto> Shots { get; set; }

        public MatchUpdateRequestDto() : base()
        {
        }

        public MatchUpdateRequestDto(
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
            type,
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
            Id = id;
            Shots = shots.Convert<ExtendedShotDto, Shot>();
        }

        public MatchUpdateRequestDto(Match match)
        : this(
            match.Id,
            Match.TypeConversion.First(x => x.Key.Equals(match.GetType())).Value,
            match.Shooter,
            match.StartTimestamp,
            match.EndTimestamp,
            match.Location,
            match.Shots,
            match.AirTemperature,
            match.AirPressure,
            match.WindSpeed,
            match.WindDirection,
            match.EnvironmentNotes,
            match.EquipmentNotes,
            match.ShooterNotes)
        {
        }

        public MatchUpdateRequestDto(MatchOutputDto outputDto)
            : this()
        {
            Id = outputDto.Id;
            Type = outputDto.Type;
            ShooterId = outputDto.ShooterId;
            StartTimestampId = outputDto.StartTimestampId;
            EndTimestampId = outputDto.EndTimestampId;
            LocationId = outputDto.LocationId;
            Shots = outputDto.Shots;
            AirTemperature = outputDto.AirTemperature;
            AirPressure = outputDto.AirPressure;
            WindSpeed = outputDto.WindSpeed;
            WindDirection = outputDto.WindDirection;
            EnvironmentNotes = outputDto.EnvironmentNotes;
            EquipmentNotes = outputDto.EquipmentNotes;
            ShooterNotes = outputDto.ShooterNotes;
        }
    }
}
