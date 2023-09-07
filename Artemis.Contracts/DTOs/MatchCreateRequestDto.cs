using System.ComponentModel.DataAnnotations;
using Artemis.Contracts.Entities;

namespace Artemis.Contracts.DTOs
{
    public class MatchCreateRequestDto
    {
        [Required(ErrorMessage = "Type is required")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Shooter is required")]
        public string ShooterId { get; set; }

        [Required(ErrorMessage = "Start time is required")]
        public string StartTimestampId { get; set; }

        [Required(ErrorMessage = "End time is required")]
        public string EndTimestampId { get; set; }

        [Required(ErrorMessage = "Location is required")]
        public string LocationId { get; set; }

        [Required(ErrorMessage = "Shots are required")]
        public List<ShotDto> Shots { get; set; }

        public double? AirTemperature { get; set; }

        public double? AirPressure { get; set; }

        public double? WindSpeed { get; set; }

        public string? WindDirection { get; set; }

        public string? EnvironmentNotes { get; set; }

        public string? EquipmentNotes { get; set; }

        public string? ShooterNotes { get; set; }

        public MatchCreateRequestDto()
        {
        }

        public MatchCreateRequestDto(
            string type,
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
        {
            Type = type;
            ShooterId = shooter.Id;
            StartTimestampId = startTimestamp.Id;
            EndTimestampId = endTimestamp.Id;
            LocationId = location.Id;
            AirTemperature = airTemperature;
            AirPressure = airPressure;
            WindSpeed = windSpeed;
            WindDirection = windDirection;
            EnvironmentNotes = environmentNotes;
            EquipmentNotes = equipmentNotes;
            ShooterNotes = shooterNotes;
        }
    }
}
