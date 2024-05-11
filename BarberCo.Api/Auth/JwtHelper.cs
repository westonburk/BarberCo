using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BarberCo.Api.Auth
{
    public class JwtHelper
    {
        public string GenerateJWTToken(SystemUser user, IConfiguration configuration)
        {
            var jwtKey = configuration.GetSection("Jwt:Key").Get<string>();
            var claims = new List<Claim> 
            {
                new (ClaimTypes.NameIdentifier, user.Id.ToString()),
                new (ClaimTypes.Name, user.Name),
            };

            var jwtToken = new JwtSecurityToken(
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddDays(30),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(
                       Encoding.UTF8.GetBytes(jwtKey)
                        ),
                    SecurityAlgorithms.HmacSha512Signature)
                );

            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return token;
        }
    }

    // TODO: change this to use 
    public class SystemUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
