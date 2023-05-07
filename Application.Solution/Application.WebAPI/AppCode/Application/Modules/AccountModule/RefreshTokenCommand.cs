using Application.WebAPI.AppCode.Application.Infrastructure;
using Application.WebAPI.AppCode.Extensions;
using Application.WebAPI.Models.DataContexts;
using Application.WebAPI.Models.Entities.Membership;
using Application.WebAPI.Models.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.WebAPI.AppCode.Application.Modules.AccountModule
{
    public class RefreshTokenCommand : IRequest<CommandJsonResponse>
    {
        public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, CommandJsonResponse>
        {
            readonly IHttpContextAccessor ctx;
            readonly UserManager<VehicleUser> userManager;
            readonly IConfiguration conf;
            readonly VehicleDbContext db;

            public RefreshTokenCommandHandler(IHttpContextAccessor ctx, UserManager<VehicleUser> userManager, IConfiguration conf, VehicleDbContext db)
            {
                this.ctx = ctx;
                this.userManager = userManager;
                this.conf = conf;
                this.db = db;
            }

            async public Task<CommandJsonResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
            {
                string cookie = ctx.HttpContext.Request.Cookies["refresh-token"];

                if (string.IsNullOrWhiteSpace(cookie))
                    return new CommandJsonResponse("Token tapılmadı!", true);

                string? token = cookie.Split(' ')[0];

                string? strUserId = cookie.Split(' ')[1].Decrypt();

                if (string.IsNullOrWhiteSpace(strUserId) || Convert.ToInt32(strUserId) <= 0)
                    return new CommandJsonResponse("İstifadəçi tapılmadı!", true);

                VehicleUser? user = await userManager.FindByIdAsync(strUserId);

                if (user is null)
                    return new CommandJsonResponse("İstifadəçi tapılmadı!", true);
                else if (user.RefreshToken != token)
                    return new CommandJsonResponse("Tokenlər xətalıdır!", true);
                else if (user.TokenExpires <= DateTime.UtcNow.AddHours(4))
                    return new CommandJsonResponse("Tokenin vaxtı bitib!", true);

                JWTTokenViewModel tokenAndExpires = user.GenerateToken(conf);
                RefreshTokenViewModel refreshToken = Extension.GenerateRefreshToken();
                await user.SetRefreshToken(refreshToken, ctx, db, cancellationToken);

                return new CommandJsonResponse<JWTTokenViewModel>("Uğurludur!", false, tokenAndExpires);
            }
        }
    }
}
