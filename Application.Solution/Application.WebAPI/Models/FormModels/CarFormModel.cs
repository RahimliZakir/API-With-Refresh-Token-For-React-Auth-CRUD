using Application.WebAPI.Models.Entities;

namespace Application.WebAPI.Models.FormModels
{
    public class CarFormModel
    {
        public int? Id { get; set; }

        public string Company { get; set; } = null!;

        public string Model { get; set; } = null!;

        public ImageItem[]? Files { get; set; }
    }
}
