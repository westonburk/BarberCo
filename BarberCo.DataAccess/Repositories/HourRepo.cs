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
            var startTime = new DateTime(2000, 12, 1, changed.StartTime.Hour, changed.StartTime.Minute, 0);
            var endTime = new DateTime(2000, 12, 1, changed.EndTime.Hour, changed.EndTime.Minute, 0);
            if (changed.IsClosed == false && startTime > endTime)
                throw new Exception($"{nameof(hour.StartTime)} cannot be before {nameof(hour.EndTime)}");

            hour.IsClosed = changed.IsClosed;
            if (hour.IsClosed)
            {
                hour.StartTime = default;
                hour.EndTime = default;
            }
            else 
            {
                hour.StartTime = startTime;
                hour.EndTime = endTime;
            }

            await _context.SaveChangesAsync(token);
            return hour;
        }
    }
}
