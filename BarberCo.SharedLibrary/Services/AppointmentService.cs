using BarberCo.SharedLibrary.Dtos;
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
        private List<Hour>? hours = null;

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

            if (date == default || date.Date < now.Date)
                return results;

            var hours = await GetHoursAsync();
            var hour = hours.First(x => x.DayOfWeek == date.DayOfWeek.ToString());
            if (hour.IsClosed)
                return results;

            var shiftStart = new DateTime(date.Year, date.Month, date.Day, hour.StartTime.Hour, 0, 0);
            var shiftEnd = new DateTime(date.Year, date.Month, date.Day, hour.EndTime.Hour, 0, 0);

            var needsPartialHours = date > shiftStart;
            if (needsPartialHours)
            {
                shiftStart = new DateTime(date.Year, date.Month, date.Day, date.Hour + 1, 0, 0);
                results.AddRange(GetHoursBetweenDates(shiftStart, shiftEnd));
            }
            else
            {
                results.AddRange(GetHoursBetweenDates(shiftStart, shiftEnd));
            }
           
           return results;
        }

        private List<(DateTime value, string display)> GetHoursBetweenDates(DateTime start, DateTime end)
        {
            var results = new List<(DateTime value, string display)>();
            while (start <= end)
            {
                results.Add((start, start.ToString("h tt")));
                start = start.AddHours(1);
            }
            return results;
        }

        public Task<Appointment> SubmitAppointmentAsync(AppointmentUpdateDto newAppointment)
        {
            return _apiService.PostAsync<AppointmentUpdateDto, Appointment>("appointment", newAppointment);
        }

        public Task<List<Appointment>> GetAppointmentsAsync()
        {
            return _apiService.GetAsync<List<Appointment>>("appointment");
        }
    }
}
