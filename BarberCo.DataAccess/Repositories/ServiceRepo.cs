using BarberCo.SharedLibrary.Dtos;
using BarberCo.SharedLibrary.Models;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Service> CreateServiceAsync(ServiceUpdateDto newService, CancellationToken token)
        {
            var service = new Service();
            service.Name = newService.Name;
            service.Price = newService.Price;
            _context.Services.Add(service);
            await _context.SaveChangesAsync(token);
            return service;
        }

        public Task DeleteServiceAsync(Service service, CancellationToken token)
        {
            _context.Services.Remove(service);
            return _context.SaveChangesAsync(token);
        }

        public Task<List<Service>> GetAllServicesAsync(CancellationToken token)
        {
            return _context.Services.ToListAsync(token);
        }

        public async Task<Service?> GetServiceByIdAsync(int Id, CancellationToken token)
        {
            return await _context.Services.FindAsync([Id], token);
        }

        public async Task<Service> UpdateServiceAsync(Service service, ServiceUpdateDto changed, CancellationToken token)
        {
            service.Name = changed.Name;
            service.Price = changed.Price;
            await _context.SaveChangesAsync(token);
            return service;
        }
    }
}
