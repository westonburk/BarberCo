using BarberCo.DataAccess.Repositories;
using BarberCo.SharedLibrary.Dtos;
using BarberCo.SharedLibrary.Exceptions;
using BarberCo.SharedLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "admin, barber")]
        public async Task<ActionResult> DeleteAppointment(int id, CancellationToken token)
        {
            try
            {
                var appointment = await _apptRepo.GetAppointmentByIdAsync(id, token);
                if (appointment == null)
                {
                    return NotFound();
                }

                await _apptRepo.DeleteAsync(appointment, token);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpPost("create/management")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "admin, barber")]
        public async Task<ActionResult<Appointment>> CreateAppointment_Barber([FromBody] AppointmentUpdateDto newAppt, CancellationToken token)
        {
            try
            {
                // TODO: this could be refactored out, and put into dependcy injection sometime, if it needs to be used again
                var createdBy = User.FindFirstValue(ClaimTypes.Name);

                var result = await _apptRepo.CreateAppointmentManagementAsync(newAppt, createdBy, token);
                if (result is null)
                {
                    return BadRequest();
                }

                return Ok(result.Id);
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

        [HttpPost("create/web")]
        [Authorize(AuthenticationSchemes = "ApiKey")]
        public async Task<ActionResult<int>> CreateAppointment([FromBody] AppointmentUpdateDto newAppt, CancellationToken token)
        {
            try
            {
                var result = await _apptRepo.CreateAppointmentWebAsync(newAppt, token);
                return Ok(result.Id);
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

        [HttpPost("confirm")]
        [Authorize(AuthenticationSchemes = "ApiKey")]
        public async Task<ActionResult<Appointment>> ConfirmAppointment([FromBody] AppointmentConfirmationDto dto, CancellationToken token)
        {
            try
            {
                var result = await _apptRepo.ConfirmAppointmentAsync(dto, token);
                if (result is null)
                {
                    return BadRequest();
                }

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
