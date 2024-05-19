using BarberCo.SharedLibrary.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BarberCo.Api.Auth
{
    public class JwtHelper
    {
        private readonly IConfiguration _config;
        private readonly UserManager<Barber> _userManager;

        public JwtHelper(IConfiguration config, UserManager<Barber> userManager)
        {
            _config = config;
            _userManager = userManager;
        }

        public async Task<string> GenerateJWTTokenAsync(Barber barber)
        {
            var jwtKey = _config.GetSection("Jwt:Key").Get<string>();
            var issuer = _config.GetSection("Jwt:Issuer").Get<string>();
            var audience = _config.GetSection("Jwt:Audience").Get<string>();

            var claims = new List<Claim>
            {
                new (ClaimTypes.NameIdentifier, barber.Id),
                new (ClaimTypes.Name, barber.UserName!),
            };

            var roles = await _userManager.GetRolesAsync(barber);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

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
