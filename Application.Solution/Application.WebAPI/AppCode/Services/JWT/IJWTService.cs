using Application.WebAPI.Models.Entities.Membership;
using System.Security.Claims;

namespace Application.WebAPI.AppCode.Services.JWT
{
    public interface IJWTService
    {
        string GenerateAccessToken(VehicleUser user);
        string GenerateRefreshToken();
        ClaimsPrincipal ValidateExpiredAccessToken(string token);
    }
}
