using Application.WebAPI.Models.DataContexts;
using Application.WebAPI.Models.Entities.Membership;
using Application.WebAPI.Models.ViewModels;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace Application.WebAPI.AppCode.Extensions
{
    public static partial class Extension
    {
        public static JWTTokenViewModel GenerateToken(this VehicleUser user, IConfiguration conf)
        {
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

            return new JWTTokenViewModel
            {
                Token = token,
                Expires = $"{expired:dd.MM.yyyy HH:mm:ss}"
            };
        }

        public static RefreshTokenViewModel GenerateRefreshToken()
        {
            RefreshTokenViewModel refreshToken = new()
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.UtcNow.AddHours(4).AddDays(3),
                //Created = DateTime.UtcNow.AddHours(4)
            };

            return refreshToken;
        }

        async public static Task SetRefreshToken(this VehicleUser user, RefreshTokenViewModel token, IHttpContextAccessor ctx, VehicleDbContext db, CancellationToken cancellationToken)
        {
            CookieOptions options = new()
            {
                HttpOnly = true,
                Expires = token.Expires
            };

            string userId = user.Id.ToString();
            string encryptedUserId = userId.Encrypt();

            ctx.HttpContext.Response.Cookies.Append("refresh-token", token.Token + " " + encryptedUserId, options);

            user.RefreshToken = token.Token;
            user.TokenExpires = token.Expires;
            user.TokenCreated = token.Created;

            await db.SaveChangesAsync(cancellationToken);
        }
    }
}
