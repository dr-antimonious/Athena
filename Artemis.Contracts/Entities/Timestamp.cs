using System.ComponentModel.DataAnnotations;

namespace Artemis.Contracts.Entities
{
    public class Timestamp
    {
        [Key]
        public string Id { get; set; }

        [Required(ErrorMessage = "Timestamp timestamp is required")]
        public DateTime TimeStamp { get; set; }

        public Timestamp()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public Timestamp(DateTime timestamp) : this()
        {
            this.TimeStamp = timestamp;
        }

        public Timestamp(string id, DateTime timeStamp)
        {
            this.Id = id;
            this.TimeStamp = timeStamp;
        }
    }
}
