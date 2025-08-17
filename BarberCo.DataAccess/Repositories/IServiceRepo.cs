using BarberCo.SharedLibrary.Dtos;
using BarberCo.SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BarberCo.DataAccess.Repositories
{
    public interface IServiceRepo
    {
        Task<List<Service>> GetAllServicesAsync(CancellationToken token);
        Task<Service?> GetServiceByIdAsync(int Id, CancellationToken token);
        Task<Service> UpdateServiceAsync(Service service, ServiceUpdateDto changed, CancellationToken token);
        Task<Service> CreateServiceAsync(ServiceUpdateDto newService, CancellationToken token);
        Task DeleteServiceAsync(Service service, CancellationToken token);
    }
}
