namespace Application.WebAPI.AppCode.Mappers.Dtos
{
    public class CarImageDto
    {
        public int Id { get; set; }

        public string? ImagePath { get; set; }

        public bool IsMain { get; set; }
    }
}
