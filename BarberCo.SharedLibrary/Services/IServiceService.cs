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
    }
}
