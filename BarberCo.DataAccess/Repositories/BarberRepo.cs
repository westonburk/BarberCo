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

        public async Task<BarberRegistrationResultDto> RegisterNewBarberAsync(BarberRegistrationDto dto)
        {
            var result = new BarberRegistrationResultDto() { Successful = false };

            if (dto.Password != dto.PasswordConfirm) 
            {
                result.Errors = $"{nameof(dto.Password)} and {dto.PasswordConfirm} did not match.";
                return result;
            }

            var newBarber = new Barber 
            { 
                UserName = dto.UserName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber
            };

            var createResult = await _userManager.CreateAsync(newBarber, dto.Password);
            if (createResult.Succeeded == false)
            {
                result.Errors = "failed to create new barber with this information.";
                return result;
            }

            var roleResult = await _userManager.AddToRoleAsync(newBarber, dto.Role);
            if (roleResult.Succeeded == false)
            {
                result.Errors = $"{dto.UserName} could not be given role {dto.Role}";
                return result;
            }

            result.Successful = true;
            result.BarberFull = newBarber;
            result.BarberDto = BarberDto.Get(newBarber);
            return result;
        }
    }
}
