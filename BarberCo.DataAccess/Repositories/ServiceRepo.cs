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
    public class ServiceRepo : IServiceRepo
    {
        private readonly DataContext _context;

        public ServiceRepo(DataContext context)
        {
            _context = context;
        }

        public Task<Service> CreateServiceAsync(ServiceUpdateDto newService, CancellationToken token)
        {
            var service = new Service();
            service.Name = newService.Name;
            service.Price = newService.Price;
            _context.Services.Add(service);
            _context.SaveChangesAsync(token);

            throw new NotImplementedException();
        }

        public Task DeleteServiceAsync(Service service, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<List<Service>> GetAllServicesAsync(CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<Service?> GetServiceByIdAsync(int Id, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<Service> UpdateServiceAsync(Service service, ServiceUpdateDto changed, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
