using BarberCo.SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarberCo.SharedLibrary.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IBarberCoApiService _apiService;
        private readonly IHourService _hourService;
        private List<Hour> hours;

        public AppointmentService(IBarberCoApiService apiService, IHourService hourService)
        {
            _apiService = apiService;
            _hourService = hourService;
        }

        private async Task<List<Hour>> GetHoursAsync()
        {
            hours ??= (await _hourService.GetAllHoursAsync())
                    .Select(x => x.hour)
                    .ToList();

            return hours;
        }

        public async Task<List<(DateTime value, string display)>> GetValidTimesForDayAsync(DateTime date)
        {
            var results = new List<(DateTime value, string display)>();
            var now = DateTime.Now;

            if (date == default || date < now)
                return results;

            var hours = await GetHoursAsync();
            var hour = hours.First(x => x.DayOfWeek == date.DayOfWeek.ToString());
            if (hour.IsClosed)
                return results;

            if (now.Date == date.Date)
            {
                var end = new DateTime(date.Year, date.Month, date.Day, hour.EndTime.Hour, 0, 0);
                results.AddRange(GetHoursBetweenDates(date.AddHours(1), end));
            }
            else
            {
                var start = new DateTime(date.Year, date.Month, date.Day, hour.StartTime.Hour, 0, 0);
                var end = new DateTime(date.Year, date.Month, date.Day, hour.EndTime.Hour, 0, 0);
                results.AddRange(GetHoursBetweenDates(start, end));
            }
           
           return results;
        }

        private List<(DateTime value, string display)> GetHoursBetweenDates(DateTime start, DateTime end)
        {
            var results = new List<(DateTime value, string display)>();
            while (start < end)
            {
                results.Add((start, start.ToString("h tt")));
                start.AddHours(1);
            }
            return results;
        }
    }
}
