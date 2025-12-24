using AvvalOnline.Shop.Api.Messaging.User;
using global::AvvalOnline.Shop.Api.Model.Entites;
using global::AvvalOnline.Shop.Api.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AvvalOnline.Shop.Api.Infrastructure
{
    public interface IJwtUtils
    {
        string GenerateToken(List<Claim> claims);
        ClaimsPrincipal ValidateToken(string token);
        int? GetUserIdFromToken(string token);
        List<string> GetUserRolesFromToken(string token);
        List<string> GetUserPermissionsFromToken(string token);
    }

    public class JwtUtils : IJwtUtils
    {
        private readonly JWTConfigModel _jwtConfig;

        public JwtUtils(JWTConfigModel jwtConfig)
        {
            _jwtConfig = jwtConfig;
        }

        public string GenerateToken(List<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtConfig.Issuer,
                audience: _jwtConfig.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_jwtConfig.SecretKey);

                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    //ValidIssuer = _jwtConfig.Issuer,
                    ValidateAudience = false,
                    //ValidAudience = _jwtConfig.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return principal;
            }
            catch
            {
                return null;
            }
        }

        public int? GetUserIdFromToken(string token)
        {
            var principal = ValidateToken(token);
            var userIdClaim = principal?.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId) ? userId : null;
        }

        public List<string> GetUserRolesFromToken(string token)
        {
            var principal = ValidateToken(token);
            return principal?.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList() ?? new List<string>();
        }

        public List<string> GetUserPermissionsFromToken(string token)
        {
            var principal = ValidateToken(token);
            return principal?.FindAll("permission").Select(c => c.Value).ToList() ?? new List<string>();
        }
    }
}
