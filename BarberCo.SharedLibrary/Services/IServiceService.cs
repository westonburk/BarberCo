using BarberCo.SharedLibrary.Dtos;
using BarberCo.SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarberCo.SharedLibrary.Services
{
    public interface IServiceService
    {
        Task<List<Service>> GetAllServicesAsync();
        Task<Service> UpdateServiceAsync(ServiceUpdateDto dto, Service service);
        Task<Service> CreateServiceAsync(ServiceUpdateDto dto);
        Task<bool> DeleteServiceAsync(Service service);
    }
}
