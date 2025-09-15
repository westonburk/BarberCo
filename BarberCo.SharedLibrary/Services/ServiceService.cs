using BarberCo.SharedLibrary.Dtos;
using BarberCo.SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarberCo.SharedLibrary.Services
{
    public class ServiceService : IServiceService
    {
        private readonly IBarberCoApiService _apiService;

        public ServiceService(IBarberCoApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<Service> CreateServiceAsync(ServiceUpdateDto dto)
        {
            var result = await _apiService.PostAsync<ServiceUpdateDto, Service>("service", dto);
            return result;
        }

        public Task<bool> DeleteServiceAsync(Service service)
        {
            return _apiService.DeleteAsync($"service/{service.Id}");
        }

        public async Task<List<Service>> GetAllServicesAsync()
        {
            return await _apiService.GetAsync<List<Service>>("service");
        }

        public async Task<Service> UpdateServiceAsync(ServiceUpdateDto dto, Service service)
        {
            var result = await _apiService.PutAsync<ServiceUpdateDto, Service>($"service/{service.Id}", dto);
            return result;
        }
    }
}
