using BarberCo.SharedLibrary.Dtos;
using BarberCo.SharedLibrary.Exceptions;
using BarberCo.SharedLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BarberCo.DataAccess.Repositories
{
    public class AppointmentRepo : IAppointmentRepo
    {
        private readonly DataContext _context;
        private readonly ITwillioRepo _twillioRepo;
        private readonly IConfiguration _config;

        public AppointmentRepo(DataContext context, ITwillioRepo twillioRepo, IConfiguration configuration)
        {
            _context = context;
            _twillioRepo = twillioRepo;
            _config = configuration;
        }

        private async Task<Appointment> AddUnConfirmedAppointmentAsync(AppointmentUpdateDto newAppointment, string createdBy, bool sendConfirmationCode, CancellationToken token)
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
                
            // TODO: more validation for phone maybe, the frontends uses masks but need extra?
            if (string.IsNullOrWhiteSpace(newAppointment.CustomerPhone))
                throw new DataValidationException($"{nameof(Appointment.CustomerPhone)} cannot be empty");

            var compareDate = new DateTime(2000, 12, 1, newAppointment.DateTime.Hour, newAppointment.DateTime.Minute, 0);
            if ((compareDate >= hour.StartTime && compareDate <= hour.EndTime) == false)
                throw new DataValidationException($"cannot create {nameof(Appointment)} outside business hours");

            var appointment = new Appointment();
            appointment.CustomerName = newAppointment.CustomerName;
            appointment.CustomerPhone = newAppointment.CustomerPhone;
            appointment.DateTime = newAppointment.DateTime;
            appointment.CreatedOn = DateTime.Now;
            appointment.CreatedBy = createdBy;
            
            if (sendConfirmationCode)
            {
                appointment.ConfirmationCodeHash = await SendConfirmationCodeAsync(appointment.CustomerPhone);
            }

            appointment.ConfirmedOn = null;
            appointment.Services.AddRange(services);
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync(token);
            return appointment;
        }

        public async Task<Appointment> CreateAppointmentWebAsync(AppointmentUpdateDto newAppointment, CancellationToken token)
        {
            var appointment = await AddUnConfirmedAppointmentAsync(newAppointment, createdBy: "Website", sendConfirmationCode: true, token);

            return appointment;
        }

        private async Task<string> SendConfirmationCodeAsync(string toPhoneNumber)
        {
            var code = GenerateConfirmationCode();
            await _twillioRepo.SendSMSAsync(new TextMessageDto 
            { 
                Message = code, ToPhoneNumber = toPhoneNumber
            });
            var hash = HashCode(code);
            return hash;
        }

        private string GenerateConfirmationCode()
        {
            int value = RandomNumberGenerator.GetInt32(0, 1_000_000);
            return value.ToString("D6");
        }

        private string HashCode(string code)
        {
            var message = Encoding.UTF8.GetBytes(code);
            var pepper = Encoding.UTF8.GetBytes(_config.GetValue<string>("SMSConfirmationCodePepper") ??
                throw new InvalidOperationException("SMSConfirmationCodePepper not configured"));

            using var hmac = new HMACSHA256(pepper);
            var hash = hmac.ComputeHash(message);
            return Convert.ToBase64String(hash);
        }

        public Task<List<Appointment>> GetAllAppointmentsAsync(CancellationToken token)
        {
            return _context.Appointments
                .Include(x => x.Services)
                .ToListAsync(token);

        }

        public async Task<Appointment> CreateAppointmentManagementAsync(AppointmentUpdateDto newAppointment, string createdByBarberId, CancellationToken token)
        {
            var appointment = await AddUnConfirmedAppointmentAsync(newAppointment, createdBy: createdByBarberId, sendConfirmationCode: false, token);
            // management appointments get confirmed by default 
            appointment.ConfirmedOn = DateTime.Now;
            await _context.SaveChangesAsync(token);
            
            return appointment;
        }

        public async Task<AppointmentUpdateDto?> ConfirmAppointmentAsync(AppointmentConfirmationDto dto, CancellationToken token)
        {
            var appt = await GetAppointmentByIdAsync(dto.AppointmentId, token);
            AppointmentUpdateDto result = null;

            if (appt is null ||
                appt.ConfirmationFailed ||
                DateTime.Now - appt.CreatedOn > TimeSpan.FromMinutes(5) ||
                appt.ConfirmedOn != null)
            {
                return null;
            }

            var hash = HashCode(dto.ConfirmationCode);

            var providedHashBytes = Convert.FromBase64String(hash);
            var storedHashBytes = appt.ConfirmationCodeHash is null
                ? Array.Empty<byte>()
                : Convert.FromBase64String(appt.ConfirmationCodeHash);

            if (CryptographicOperations.FixedTimeEquals(storedHashBytes, providedHashBytes))
            {
                appt.ConfirmedOn = DateTime.Now;
                result = new AppointmentUpdateDto
                {
                    CustomerName = appt.CustomerName,
                    CustomerPhone = appt.CustomerPhone,
                    DateTime = appt.DateTime,
                    ServiceIds = appt.Services.Select(x => x.Id).ToList()
                };

            }
            else
            {
                appt.ConfirmationFailed = true;
            }

            await _context.SaveChangesAsync(token);
            return result;
        }

        public async Task<Appointment?> GetAppointmentByIdAsync(int id, CancellationToken token)
        {
            return await _context.Appointments
                .Include(x => x.Services)
                .FirstOrDefaultAsync(x => x.Id == id, token);
        }

        public Task DeleteAsync(Appointment appointment, CancellationToken token)
        {
            _context.Appointments.Remove(appointment);
            return _context.SaveChangesAsync(token);
        }
    }
}
