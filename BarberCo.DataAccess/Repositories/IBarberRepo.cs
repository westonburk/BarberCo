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
        Task<BarberRegistrationResultDto> RegisterNewBarberAsync(BarberRegistrationDto dto);
        Task<List<BarberDto>> GetAllBarbersAsync(CancellationToken token, bool includeDeleted = false);
    }
}
