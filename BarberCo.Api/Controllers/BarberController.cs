using BarberCo.Api.Dtos;
using BarberCo.DataAccess.Repositories;
using BarberCo.SharedLibrary.Dtos;
using BarberCo.SharedLibrary.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
        private readonly IBarberRepo _barberRepo;
        private readonly ILogger<Barber> _logger;

        public BarberController(IBarberRepo barberRepo, ILogger<Barber> logger)
        {
            _barberRepo = barberRepo;
            _logger = logger;
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "admin")]
        [HttpPost("register")]
        public async Task<ActionResult<BarberDto>> RegisterBarber([FromBody] BarberRegistrationDto barberDto, CancellationToken token)
        {
            try
            {
                var result = await _barberRepo.RegisterNewBarberAsync(barberDto);
                if (result.Errors != null)
                {
                    return BadRequest(result.Errors);
                }

                return Ok(result.BarberDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer,ApiKey")]
        public async Task<ActionResult<List<BarberDto>>> GetBarbers(CancellationToken token)
        {
            try
            {
                var dtos = await _barberRepo.GetAllBarbersAsync(token);
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer,ApiKey")]
        public async Task<ActionResult<List<BarberDto>>> GetBarber(CancellationToken token)
        {
            try
            {
                var dtos = await _barberRepo.GetAllBarbersAsync(token);
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        // put to change password
        // put to change other fields
        // delete
    }
}
