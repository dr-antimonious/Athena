using System.ComponentModel.DataAnnotations;
using Artemis.Contracts.Entities;

namespace Artemis.Contracts.DTOs
{
    public class ExtendedShotDto : ShotDto
    {
        [Required(ErrorMessage = "Shot id is required")]
        public string Id { get; set; }

        public override Shot Convert()
            => new(this);

        public ExtendedShotDto() : base()
        {
        }

        public ExtendedShotDto(
            string id,
            double value,
            int position,
            Timestamp? timestamp = null,
            double? horizontalDisplacement = null,
            double? verticalDisplacement = null)
        : base(
            value,
            position,
            timestamp,
            horizontalDisplacement,
            verticalDisplacement)
        {
            Id = id;
        }

        public ExtendedShotDto(Shot shot)
            : this(
                shot.Id,
                shot.Value,
                shot.Position,
                shot.TimeStamp,
                shot.HorizontalDisplacement,
                shot.VerticalDisplacement)
        {
        }
    }
}
