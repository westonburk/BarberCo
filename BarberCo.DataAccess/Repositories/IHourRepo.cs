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
    public interface IHourRepo
    {
        Task<List<Hour>> GetAllHoursAsync(CancellationToken token);
        Task<Hour?> GetHourByIdAsync(int Id, CancellationToken token);
        Task<Hour> UpdateHourAsync(Hour hour, HourUpdateDto changed, CancellationToken token);
    }
}
