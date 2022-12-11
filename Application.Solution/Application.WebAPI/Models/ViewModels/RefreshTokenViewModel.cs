namespace Application.WebAPI.Models.ViewModels
{
    public class RefreshTokenViewModel
    {
        public string? Token { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow.AddHours(4);

        public DateTime Expires { get; set; }
    }
}
