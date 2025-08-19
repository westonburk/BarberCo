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

        public async Task<List<Service>> GetAllServicesAsync()
        {
            return await _apiService.GetAsync<List<Service>>("service");
        }
    }
}
