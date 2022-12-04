namespace Application.WebAPI.Models.Entities
{
    public class CarImage
    {
        public int Id { get; set; }

        public string? ImagePath { get; set; }

        public bool IsMain { get; set; }

        public int CarId { get; set; }

        public virtual Car Car { get; set; } = null!;
    }
}
