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
    public interface IAppointmentRepo
    {
        Task<List<Appointment>> GetAllAppointmentsAsync(CancellationToken token);
        Task<Appointment> CreateAppointmentWebAsync(AppointmentUpdateDto newAppointment, CancellationToken token);
        /// <summary>
        /// For creating from admin panel, no sms confirmation
        /// </summary>
        /// <param name="newAppointment"></param>
        /// <param name="barber"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<Appointment> CreateAppointmentManagementAsync(AppointmentUpdateDto newAppointment, string createdByBarberId, CancellationToken token);
        Task<AppointmentUpdateDto?> ConfirmAppointmentAsync(AppointmentConfirmationDto dto, CancellationToken token);
        Task<Appointment?> GetAppointmentByIdAsync(int id, CancellationToken token);
        Task DeleteAsync(Appointment appointment, CancellationToken token);
    }
}
