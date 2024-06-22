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
    public interface IBarberRepo
    {
        Task<BarberResultDto> RegisterNewBarberAsync(BarberRegistrationDto dto);
        Task<List<BarberDto>> GetAllBarbersAsync(CancellationToken token, bool includeDeleted = false);
        Task<BarberDto?> GetByIdAsync(string id);
        Task<string> ChangePasswordAsync(Barber barber, BarberChangePasswordDto dto);
        Task<BarberResultDto> UpdateAsync(BarberDto changed, Barber original);
        Task<BarberResultDto> DeleteAsync(Barber barber);
    }
}
