using System.ComponentModel.DataAnnotations;

namespace Artemis.Contracts.DTOs
{
    public class LocationCreateRequestDto
    {
        [Required(ErrorMessage = "Location name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Country is required")]
        public string CountryId { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string CityId { get; set; }
    }
}
