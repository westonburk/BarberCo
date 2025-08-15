using BarberCo.SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarberCo.SharedLibrary.Services
{
    public interface IHourService
    {
        Task<List<(int sort, Hour hour)>> GetAllHoursAsync();
    }
}
