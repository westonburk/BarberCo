using BarberCo.SharedLibrary.Dtos;
using BarberCo.SharedLibrary.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BarberCo.DataAccess.Repositories
{
    public class BarberRepo : IBarberRepo
    {
        private readonly UserManager<Barber> _userManager;

        public BarberRepo(UserManager<Barber> userManager)
        {
            _userManager = userManager;
        }

        public Task<List<BarberDto>> GetAllBarbersAsync(CancellationToken token, bool includeDeleted = false)
        {
            if (includeDeleted)
            {
                return _userManager.Users
                    .Select(x => BarberDto.Get(x))
                    .ToListAsync(token);
            }

            return _userManager.Users
                .Where(x => x.DeletedOn == null)
                .Select(x => BarberDto.Get(x))
                .ToListAsync(token);
        }

        public BarberRegistrationResultDto RegisterNewBarberAsync(BarberRegistrationDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
