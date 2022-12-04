using Application.WebAPI.Models.Entities;

namespace Application.WebAPI.AppCode.Mappers.Dtos
{
    public class CarDto
    {
        public int Id { get; set; }

        public string Company { get; set; } = null!;

        public string Model { get; set; } = null!;

        public ICollection<CarImageDto>? CarImages { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
