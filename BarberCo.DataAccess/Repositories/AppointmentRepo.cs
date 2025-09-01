using BarberCo.SharedLibrary.Dtos;
using BarberCo.SharedLibrary.Exceptions;
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
    public class AppointmentRepo : IAppointmentRepo
    {
        private readonly DataContext _context;

        public AppointmentRepo(DataContext context)
        {
            _context = context;
        }

        public async Task<Appointment> CreateAppointmentAsync(AppointmentUpdateDto newAppointment, CancellationToken token)
        {
            if (newAppointment.DateTime < DateTime.Now)
                throw new DataValidationException($"cannot create {nameof(Appointment)} invalid time");

            var dayOfWeek = newAppointment.DateTime.DayOfWeek.ToString();
            var hour = await _context.Hours.FirstAsync(x => x.DayOfWeek == dayOfWeek, token);
            if (hour.IsClosed)
                throw new DataValidationException($"{dayOfWeek} is closed cannot create {nameof(Appointment)}");

            var services = await _context.Services
                .Where(x => newAppointment.ServiceIds.Contains(x.Id))
                .ToListAsync(token);

            if (services.Count == 0)
                throw new DataValidationException($"cannot create {nameof(Appointment)} without service(s)");

            if (string.IsNullOrWhiteSpace(newAppointment.CustomerName))
                throw new DataValidationException($"{nameof(Appointment.CustomerName)} cannot be empty");
                
            if (string.IsNullOrWhiteSpace(newAppointment.CustomerPhone))
                throw new DataValidationException($"{nameof(Appointment.CustomerPhone)} cannot be empty");

            var compareDate = new DateTime(2000, 12, 1, newAppointment.DateTime.Hour, newAppointment.DateTime.Minute, 0);
            if ((compareDate >= hour.StartTime && compareDate <= hour.EndTime) == false)
                throw new DataValidationException($"cannot create {nameof(Appointment)} outside business hours");

            var appointment = new Appointment();
            appointment.CustomerName = newAppointment.CustomerName;
            appointment.CustomerPhone = newAppointment.CustomerPhone;
            appointment.DateTime = newAppointment.DateTime;
            appointment.Services.AddRange(services);
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync(token);
            return appointment;
        }

        public Task<List<Appointment>> GetAllAppointmentsAsync(CancellationToken token)
        {
            return _context.Appointments
                .Include(x => x.Services)
                .ToListAsync(token);

        }
    }
}
