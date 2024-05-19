using BarberCo.Api.Auth;
using BarberCo.Api.Dtos;
using BarberCo.SharedLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BarberCo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<Barber> _userManager;
        private readonly JwtHelper _jwtHelper;
        private readonly SignInManager<Barber> _signInManager;

        public AuthController(UserManager<Barber> userManager, JwtHelper jwtHelper, SignInManager<Barber> signInManager)
        {
            _userManager = userManager;
            _jwtHelper = jwtHelper;
            _signInManager = signInManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
                return Unauthorized(new { message = "Invalid credentials" });

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded)
                return Unauthorized(new { message = "Invalid credentials" });

            var token = await _jwtHelper.GenerateJWTTokenAsync(user);
            return Ok(new { token });
        }
    }
}
