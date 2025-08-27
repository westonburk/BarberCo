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
        Task<Appointment> CreateAppointmentAsync(AppointmentUpdateDto newAppointment, CancellationToken token);
    }
}
