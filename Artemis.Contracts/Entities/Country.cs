using System.ComponentModel.DataAnnotations;

namespace Artemis.Contracts.Entities
{
    public class Country
    {
        [Key]
        public string Id { get; set; }

        [Required(ErrorMessage = "Country name is required")]
        public string Name { get; set; } = null!;

        public List<City> Cities { get; set; }

        public List<Location> Locations { get; set; }

        public Country()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Cities = new List<City>();
            this.Locations = new List<Location>();
        }

        public Country(string name)
            : this()
        {
            this.Name = name;
        }

        public Country(
            string id, 
            string name, 
            List<City> cities,
            List<Location> locations)
        {
            this.Id = id;
            this.Name = name;
            this.Cities = cities;
            this.Locations = locations;
        }
    }
}
