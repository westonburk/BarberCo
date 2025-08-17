using BarberCo.DataAccess.Repositories;
using BarberCo.SharedLibrary.Dtos;
using BarberCo.SharedLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarberCo.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : Controller
    {
        private readonly IServiceRepo _serviceRepo;
        private readonly ILogger<Service> _logger;

        public ServiceController(IServiceRepo serviceRepo, ILogger<Service> logger)
        {
            _serviceRepo = serviceRepo;
            _logger = logger;
        }

        [HttpGet()]
        [Authorize(AuthenticationSchemes = "Bearer,ApiKey")]
        public async Task<ActionResult<List<Service>>> GetAllServices(CancellationToken token)
        {
            try
            {
                var services = await _serviceRepo.GetAllServicesAsync(token);
                return Ok(services);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<Service>> PutService(int id, ServiceUpdateDto service, CancellationToken token)
        {
            try
            {
                var original = await _serviceRepo.GetServiceByIdAsync(id, token);
                if (original is null)
                {
                    return NotFound();
                }

                var result = await _serviceRepo.UpdateServiceAsync(original, service, token);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<Service>> PostService(ServiceUpdateDto newService, CancellationToken token)
        {
            try
            {
                var result = await _serviceRepo.CreateServiceAsync(newService, token);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }


        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult> DeleteService(int id, CancellationToken token)
        {
            try
            {
                var service = await _serviceRepo.GetServiceByIdAsync(id, token);
                if (service == null)
                {
                    return NotFound();
                }

                await _serviceRepo.DeleteServiceAsync(service,token);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }
    }
}
