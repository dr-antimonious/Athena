using System.ComponentModel.DataAnnotations;

namespace Artemis.Contracts.DTOs
{
    public class CityCreateRequestDto
    {
        [Required(ErrorMessage = "City name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Country is required")]
        public string Id { get; set; }
    }
}
