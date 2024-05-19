using BarberCo.Api.Dtos;
using BarberCo.SharedLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BarberCo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BarberController : ControllerBase
    {
        private readonly UserManager<Barber> _userManager;

        public BarberController(UserManager<Barber> userManager)
        {
            _userManager = userManager;
        }

        [Authorize(Roles = "admin")]
        [HttpPost("register")]
        public async Task<ActionResult<Barber>> RegisterBarber([FromBody] BarberRegistrationDto barberDto)
        {
            if (barberDto.Password != barberDto.PasswordConfirm)
            {
                return BadRequest($"{nameof(barberDto.Password)} and {barberDto.PasswordConfirm} did not match.");
            }

            // TODO: refactor this into a repo
            var newBarber = new Barber { UserName = barberDto.UserName, Email = barberDto.Email, PhoneNumber = barberDto.PhoneNumber };
            var result = await _userManager.CreateAsync(newBarber, barberDto.Password);
            if (result.Succeeded == false)
            {
                return BadRequest(result.Errors);
            }

            var roleResult = await _userManager.AddToRoleAsync(newBarber, barberDto.Role);
            if (roleResult.Succeeded == false)
            {
                return BadRequest(roleResult.Errors);
            }

            return Ok(newBarber);
        }

        [HttpGet]
        public async Task<ActionResult<List<Barber>>> GetBarbers()
        {
            // TODO: fetch all barbers from repo
            await Task.CompletedTask;

            return Ok(new List<Barber>());
        }
    }
}
