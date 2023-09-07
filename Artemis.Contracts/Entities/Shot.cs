using System.ComponentModel.DataAnnotations;
using Artemis.Contracts.DTOs;
using Artemis.Contracts.Entities.Interfaces;

namespace Artemis.Contracts.Entities
{
    public class Shot : IConvertable<ExtendedShotDto>
    {
        [Key]
        public string Id { get; }

        [Required(ErrorMessage = "Shot position is required"),
        Range(1, 120)]
        public int Position { get; set; }

        public Timestamp? TimeStamp { get; set; }

        [Required(ErrorMessage = "Shot value is required")]
        public double Value { get; set; }

        public double? HorizontalDisplacement { get; set; }

        public double? VerticalDisplacement { get; set; }

        public ExtendedShotDto Convert()
            => new(this);

        public Shot()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public Shot(
            double value,
            int position,
            Timestamp? timeStamp = null,
            double? horizontalDisplacement = null,
            double? verticalDisplacement = null)
            : this()
        {
            this.Value = value;
            this.Position = position;
            this.TimeStamp = timeStamp;
            this.HorizontalDisplacement = horizontalDisplacement;
            this.VerticalDisplacement = verticalDisplacement;
        }

        public Shot(
            string id, 
            double value,
            int position,
            Timestamp? timeStamp = null, 
            double? horizontalDisplacement = null, 
            double? verticalDisplacement = null)
        {
            this.Id = id;
            this.TimeStamp = timeStamp;
            this.Value = value;
            this.Position = position;
            this.HorizontalDisplacement = horizontalDisplacement;
            this.VerticalDisplacement = verticalDisplacement;
        }

        public Shot(ShotDto dto)
            : this(
                dto.Value,
                dto.Position,
                dto.Timestamp,
                dto.HorizontalDisplacement,
                dto.VerticalDisplacement)
        {
        }

        public Shot(ExtendedShotDto dto)
            : this(
                dto.Id,
                dto.Value,
                dto.Position,
                dto.Timestamp,
                dto.HorizontalDisplacement,
                dto.VerticalDisplacement)
        {
        }
    }
}
