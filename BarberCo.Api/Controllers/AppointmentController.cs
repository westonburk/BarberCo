using BarberCo.DataAccess.Repositories;
using BarberCo.SharedLibrary.Dtos;
using BarberCo.SharedLibrary.Exceptions;
using BarberCo.SharedLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarberCo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : Controller
    {
        private readonly IAppointmentRepo _apptRepo;
        private readonly ILogger<Appointment> _logger;

        public AppointmentController(IAppointmentRepo apptRepo, ILogger<Appointment> logger)
        {
            _apptRepo = apptRepo;
            _logger = logger;
        }

        [HttpGet()]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<List<Appointment>>> GetAllAppointments(CancellationToken token)
        {
            try
            {
                var appointments = await _apptRepo.GetAllAppointmentsAsync(token);
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "ApiKey")]
        public async Task<ActionResult<Appointment>> PostAppointment([FromBody] AppointmentUpdateDto newAppt, CancellationToken token)
        {
            try
            {
                var result = await _apptRepo.CreateAppointmentAsync(newAppt, token);
                return Ok(result);
            }
            catch (DataValidationException ex)
            {
                return StatusCode(StatusCodes.Status422UnprocessableEntity, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }
    }
}
