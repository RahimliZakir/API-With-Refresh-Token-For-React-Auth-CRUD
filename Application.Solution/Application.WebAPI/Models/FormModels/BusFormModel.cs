namespace Application.WebAPI.Models.FormModels
{
    public class BusFormModel
    {
        public int? Id { get; set; }

        public string Company { get; set; } = null!;

        public string Model { get; set; } = null!;

        public IFormFile? File { get; set; }

        public string? FileTemp { get; set; }
    }
}
