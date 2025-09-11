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

        public async Task<BarberResultDto> ChangePasswordAsync(Barber barber, BarberChangePasswordDto dto)
        {
            var result = new BarberResultDto();

            var identityResult = await _userManager.ChangePasswordAsync(barber,dto.CurrentPassword, dto.NewPassword);
            if (identityResult.Succeeded)
            {
                result.BarberDto = await GetByIdAsync(barber.Id);
                return result;
            }

            result.Errors = $"failed to change password for {barber.Id}";
            return result;
        }

        public async Task<BarberResultDto> DeleteAsync(Barber barber)
        {
            var result = new BarberResultDto();

            barber.DeletedOn = DateTime.Now;
            barber.IsAvailable = false;
            var update = await _userManager.UpdateAsync(barber);
            if (update.Succeeded)
            {
                result.BarberDto = BarberDto.Get(barber);
                return result;
            }

            result.Errors = $"failed to delete {barber.Id}";
            return result;
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

        public async Task<BarberDto?> GetByIdAsync(string id)
        {
            var result = await _userManager.FindByIdAsync(id);
            if (result == null)
                return null;

            var dto = BarberDto.Get(result);
            return dto;
        }

        public async Task<Barber?> GetFullBarberByIdAsync(string id)
        {
            var result = await _userManager.FindByIdAsync(id);
            return result;
        }

        public async Task<BarberResultDto> RegisterNewBarberAsync(BarberRegistrationDto dto)
        {
            var result = new BarberResultDto();

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
                result.Errors = string.Join($"{Environment.NewLine}", createResult.Errors.Select(x => x.Description));
                return result;
            }

            var roleResult = await _userManager.AddToRoleAsync(newBarber, dto.Role);
            if (roleResult.Succeeded == false)
            {
                result.Errors = $"{dto.UserName} could not be given role {dto.Role}";
                return result;
            }

            result.BarberDto = BarberDto.Get(newBarber);
            return result;
        }

        public async Task<BarberResultDto> UpdateAsync(BarberDto changed, Barber original)
        {
            var result = new BarberResultDto();

            original.PhoneNumber = changed.PhoneNumber;
            original.UserName = changed.UserName;
            original.Email = changed.Email;
            original.IsAvailable = changed.IsAvaliable;

            var update = await _userManager.UpdateAsync(original);
            if (update.Succeeded)
            {
                result.BarberDto = await GetByIdAsync(changed.Id);
                return result;
            }

            result.Errors = $"failed to update {changed.Id}";
            return result;
        }
    }
}
