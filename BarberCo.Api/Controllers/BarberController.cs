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

        [HttpGet("all")]
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

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer,ApiKey")]
        public async Task<ActionResult<BarberDto>> GetBarber(string id)
        {
            try
            {
                var dto = await _barberRepo.GetByIdAsync(id);
                if (dto == null)
                {
                    return NotFound();
                }

                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<BarberDto>> PutBarber(string id, BarberDto dto)
        {
            try
            {
                var barber = await _barberRepo.GetFullBarberByIdAsync(id);
                if (barber == null)
                {
                    return NotFound();
                }

                var result = await _barberRepo.UpdateAsync(dto, barber);
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

        [HttpPut("{id}/password")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<BarberResultDto>> ChangeBarberPassword(string id, BarberChangePasswordDto dto)
        {
            try
            {
                var barber = await _barberRepo.GetFullBarberByIdAsync(id);
                if (barber == null)
                {
                    return NotFound();
                }

                var result = await _barberRepo.ChangePasswordAsync(barber, dto);
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

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "admin")]
        public async Task<ActionResult<BarberResultDto>> DeleteBarber(string id)
        {
            try
            {
                var barber = await _barberRepo.GetFullBarberByIdAsync(id);
                if (barber == null)
                {
                    return NotFound();
                }

                var result = await _barberRepo.DeleteAsync(barber);
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
    }
}
