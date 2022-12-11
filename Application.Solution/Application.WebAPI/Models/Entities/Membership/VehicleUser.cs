using Microsoft.AspNetCore.Identity;

namespace Application.WebAPI.Models.Entities.Membership
{
    public class VehicleUser : IdentityUser<int>
    {
        public string? Name { get; set; }

        public string? Surname { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime TokenCreated { get; set; }

        public DateTime TokenExpires { get; set; }
    }
}
