using BarberCo.DataAccess.Repositories;
using BarberCo.SharedLibrary.Dtos;
using BarberCo.SharedLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarberCo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HourController : Controller
    {
        private readonly IHourRepo _hourRepo;
        private readonly ILogger<Hour> _logger;

        public HourController(IHourRepo hourRepo, ILogger<Hour> logger)
        {
            _hourRepo = hourRepo;
            _logger = logger;
        }

        [HttpGet()]
        [Authorize(AuthenticationSchemes = "Bearer,ApiKey")]
        public async Task<ActionResult<List<Hour>>> GetAllHours(CancellationToken token)
        {
            try
            {
                var hours = await _hourRepo.GetAllHoursAsync(token);
                return Ok(hours);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<Hour>> PutHour(int id, [FromBody] HourUpdateDto hour, CancellationToken token)
        {
            try
            {
                var original = await _hourRepo.GetHourByIdAsync(id, token);
                if (original is null)
                {
                    return NotFound();
                }

                var result = await _hourRepo.UpdateHourAsync(original, hour, token);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }
    }
}
