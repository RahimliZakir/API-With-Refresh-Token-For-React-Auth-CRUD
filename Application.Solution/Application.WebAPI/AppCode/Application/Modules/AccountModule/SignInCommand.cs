using Application.WebAPI.AppCode.Application.Infrastructure;
using Application.WebAPI.AppCode.Extensions;
using Application.WebAPI.Models.DataContexts;
using Application.WebAPI.Models.Entities.Membership;
using Application.WebAPI.Models.FormModels;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.WebAPI.AppCode.Application.Modules.AccountModule
{
    public class SignInCommand : SignInFormModel, IRequest<CommandJsonResponse>
    {
        public class SignInCommandHandler : IRequestHandler<SignInCommand, CommandJsonResponse>
        {
            readonly VehicleDbContext db;
            readonly UserManager<VehicleUser> userManager;
            readonly SignInManager<VehicleUser> signInManager;
            readonly IConfiguration conf;

            public SignInCommandHandler(VehicleDbContext db, UserManager<VehicleUser> userManager, SignInManager<VehicleUser> signInManager, IConfiguration conf)
            {
                this.db = db;
                this.userManager = userManager;
                this.signInManager = signInManager;
                this.conf = conf;
            }

            async public Task<CommandJsonResponse> Handle(SignInCommand request, CancellationToken cancellationToken)
            {
                if (string.IsNullOrWhiteSpace(request.Username))
                {
                    return new CommandJsonResponse("İstifadəçi adı göndərilməyib!", true);
                }
                else if (string.IsNullOrWhiteSpace(request.Password))
                {
                    return new CommandJsonResponse("Şifrə göndərilməyib!", true);
                }

                VehicleUser user = null;
                if (request.Username.IsEmail())
                {
                    user = await userManager.FindByEmailAsync(request.Username);
                }
                else
                {
                    user = await userManager.FindByNameAsync(request.Username);
                }

                if (user == null)
                {
                    return new CommandJsonResponse("İstifadəçi adı və ya şifrə xətalıdır!", true);
                }

                SignInResult result = await signInManager.CheckPasswordSignInAsync(user, request.Password, true);

                if (result.IsLockedOut)
                {
                    return new CommandJsonResponse("Hesabınız keçici olaraq məhdudlaşıdırılıb!", true);
                }
                else if (result.IsNotAllowed)
                {
                    return new CommandJsonResponse("Hesabınız məhdudlaşıdırılıb!", true);
                }
                else if (result.Succeeded)
                    goto stopGenerate;
                else
                {
                    return new CommandJsonResponse("Giriş hüququnuz yoxdur!", true);
                }

            stopGenerate:
                List<Claim> claims = new();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));

                if (!string.IsNullOrWhiteSpace(user.Name) || !string.IsNullOrWhiteSpace(user.Surname))
                {
                    claims.Add(new Claim("FullName", $"{user.Name} {user.Surname}"));
                }
                else if (!string.IsNullOrWhiteSpace(user.PhoneNumber))
                {
                    claims.Add(new Claim("FullName", $"{user.PhoneNumber}"));
                }
                else
                {
                    claims.Add(new Claim("FullName", $"{user.Email}"));
                }

                claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

                string issuer = conf["Jwt:Issuer"];
                string audience = conf.GetValue<string>("Jwt:Audience");

                byte[] buffer = Encoding.UTF8.GetBytes(conf["Jwt:Secret"]);
                SymmetricSecurityKey key = new(buffer);
                SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);

                DateTime expired = DateTime.UtcNow.AddHours(4).AddMinutes(10);

                JwtSecurityToken tokenBuilder = new(issuer, audience, claims,
                                                        expires: expired,
                                                        signingCredentials: creds);

                string token = new JwtSecurityTokenHandler().WriteToken(tokenBuilder);

                return new CommandJsonResponse<string>("Uğurludur!", false, $"Token: {token}, expired: {expired}.");
            }
        }
    }
}
