using Application.WebAPI.Models.Entities.Membership;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Application.WebAPI.AppCode.Services.JWT
{
    public class JWTService : IJWTService
    {
        readonly IConfiguration conf;
        public JWTService(IConfiguration conf)
        {
            this.conf = conf;
        }

        public string GenerateAccessToken(VehicleUser user)
        {
            try
            {
                List<Claim> claims = new()
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                };

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

                string issuer = conf["JWT:Issuer"];
                string audience = conf.GetValue<string>("JWT:Audience");

                byte[] buffer = Encoding.UTF8.GetBytes(conf["JWT:Secret"]);
                var key = new SymmetricSecurityKey(buffer);
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var expired = DateTime.UtcNow.AddMinutes(30);

                var tokenBuilder = new JwtSecurityToken(issuer, audience, claims,
                                                        expires: expired,
                                                        signingCredentials: creds);

                string token = new JwtSecurityTokenHandler().WriteToken(tokenBuilder);

                return token;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string GenerateRefreshToken()
        {
            try
            {
                byte[] randomNumber = new byte[64];

                using RandomNumberGenerator rng = RandomNumberGenerator.Create();
                rng.GetBytes(randomNumber);

                return Convert.ToBase64String(randomNumber);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ClaimsPrincipal ValidateExpiredAccessToken(string accessToken)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(conf.GetValue<string>("Jwt:Secret"));

            TokenValidationParameters tokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = conf.GetValue<string>("Jwt:Issuer"),
                ValidAudience = conf.GetValue<string>("Jwt:Audience"),
                IssuerSigningKey = new SymmetricSecurityKey(buffer)
            };

            JwtSecurityTokenHandler tokenHandler = new();
            var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                return null;

            return principal;

        }
    }
}
