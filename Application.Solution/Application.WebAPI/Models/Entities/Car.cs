using System.ComponentModel.DataAnnotations.Schema;

namespace Application.WebAPI.Models.Entities
{
    public class Car : BaseEntity
    {
        public string Company { get; set; } = null!;

        public string Model { get; set; } = null!;

        [NotMapped]
        public ImageItem[]? Files { get; set; }
    }
}
