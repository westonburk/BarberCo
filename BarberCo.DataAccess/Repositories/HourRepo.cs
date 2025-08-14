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
    public class HourRepo : IHourRepo
    {
        private readonly DataContext _context;

        public HourRepo(DataContext context)
        {
            _context = context;
        }

        public Task<List<Hour>> GetAllHoursAsync(CancellationToken token)
        {
            return _context.Hours.ToListAsync(token);
        }

        public async Task<Hour?> GetHourByIdAsync(int Id, CancellationToken token)
        {
            var result = await _context.Hours.FindAsync([Id], cancellationToken: token);
            return result;
        }

        public async Task<Hour> UpdateHourAsync(Hour hour, HourUpdateDto changed, CancellationToken token)
        {
            hour.StartTime = changed.StartTime;
            hour.EndTime = changed.EndTime;

            await _context.SaveChangesAsync(token);
            return GetHourByIdAsync(hour.Id, token).Result;
        }
    }
}
