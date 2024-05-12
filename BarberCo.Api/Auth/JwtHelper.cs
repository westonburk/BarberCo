using BarberCo.SharedLibrary.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BarberCo.Api.Auth
{
    public class JwtHelper
    {
        public string GenerateJWTToken(Barber barber, IConfiguration configuration)
        {
            var jwtKey = configuration.GetSection("Jwt:Key").Get<string>();
            var issuer = configuration.GetSection("Jwt:Issuer").Get<string>();
            var audience = configuration.GetSection("Jwt:Audience").Get<string>();

            var claims = new List<Claim>
            {
                new (ClaimTypes.NameIdentifier, barber.Id),
                new (ClaimTypes.Name, barber.UserName!),
            };

            var jwtToken = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddDays(30),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!)),
                    SecurityAlgorithms.HmacSha512Signature)
            );

            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return token;
        }
    }
}
