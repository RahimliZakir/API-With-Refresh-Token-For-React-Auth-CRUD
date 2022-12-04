using System.ComponentModel.DataAnnotations.Schema;

namespace Application.WebAPI.Models.Entities
{
    public class Bus : BaseEntity
    {
        public string Company { get; set; } = null!;

        public string Model { get; set; } = null!;

        public string? ImagePath { get; set; }

        [NotMapped]
        public string? FileTemp { get; set; }
    }
}
