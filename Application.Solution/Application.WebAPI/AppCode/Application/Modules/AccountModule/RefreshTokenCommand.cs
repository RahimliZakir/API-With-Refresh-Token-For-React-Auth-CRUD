using Application.WebAPI.AppCode.Application.Infrastructure;
using Application.WebAPI.AppCode.Extensions;
using Application.WebAPI.AppCode.Services.JWT;
using Application.WebAPI.Models.DataContexts;
using Application.WebAPI.Models.Entities.Membership;
using Application.WebAPI.Models.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Application.WebAPI.AppCode.Application.Modules.AccountModule
{
    public class RefreshTokenCommand : IRequest<CommandJsonResponse>
    {
        [Required(ErrorMessage = "Access token göndərilməlidir!")]
        public string AccessToken { get; set; } = null!;
        [Required(ErrorMessage = "Refresh token göndərilməlidir!")]
        public string RefreshToken { get; set; } = null!;

        public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, CommandJsonResponse>
        {
            readonly IHttpContextAccessor ctx;
            readonly UserManager<VehicleUser> userManager;
            readonly IConfiguration conf;
            readonly VehicleDbContext db;
            readonly IJWTService jwtService;

            public RefreshTokenCommandHandler(IHttpContextAccessor ctx, UserManager<VehicleUser> userManager, IConfiguration conf, VehicleDbContext db, IJWTService jwtService)
            {
                this.ctx = ctx;
                this.userManager = userManager;
                this.conf = conf;
                this.db = db;
                this.jwtService = jwtService;
            }

            async public Task<CommandJsonResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
            {
                DateTime nowAz = DateTime.UtcNow.AddHours(4);

                var principal = jwtService.ValidateExpiredAccessToken(request.AccessToken);

                if (principal == null)
                    goto userNotFound;

                int? userId = principal.GetUserId();

                if (userId == null)
                    goto userNotFound;

                VehicleUser user = await db.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken); ;

                if (user == null)
                    goto userNotFound;

                if (string.IsNullOrWhiteSpace(user.RefreshToken) || user.RefreshToken?.Decrypt() != request.RefreshToken)
                    return new CommandJsonResponse("Refresh Token-də xəta var!", true);

                if (user.TokenExpires <= nowAz)
                    return new CommandJsonResponse("Refresh Token-in vaxtı bitmişdir!", true);

                JWTTokenViewModel vm = new()
                {
                    AccessToken = jwtService.GenerateAccessToken(user),
                    RefreshToken = jwtService.GenerateRefreshToken()
                };

                if (!string.IsNullOrWhiteSpace(vm.RefreshToken))
                {
                    string encryptedRefreshToken = vm.RefreshToken.Encrypt();

                    user.RefreshToken = encryptedRefreshToken;
                    user.TokenCreated = nowAz;
                    user.TokenExpires = nowAz.AddDays(1);

                    await db.SaveChangesAsync(cancellationToken);
                }

                return new CommandJsonResponse<JWTTokenViewModel>("Belə bir istifadəçi tapılmadı!", false, vm);

            userNotFound:
                return new CommandJsonResponse("Belə bir istifadəçi tapılmadı!", true);
            }
        }
    }
}
