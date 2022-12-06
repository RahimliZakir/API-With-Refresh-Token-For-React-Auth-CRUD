namespace Application.WebAPI.AppCode.Mappers.Dtos
{
    public class TruckDto
    {
        public int Id { get; set; }

        public string Company { get; set; } = null!;

        public string Model { get; set; } = null!;

        public string? ImagePath { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
