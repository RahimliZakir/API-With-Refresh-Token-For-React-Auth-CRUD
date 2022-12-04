using System.ComponentModel.DataAnnotations.Schema;

namespace Application.WebAPI.Models.Entities
{
    public class Car : BaseEntity
    {
        public string Company { get; set; } = null!;

        public string Model { get; set; } = null!;

        public virtual ICollection<CarImage>? CarImages { get; set; }

        [NotMapped]
        public ImageItem[]? Files { get; set; }
    }
}
