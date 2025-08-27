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
    public class AppointmentRepo : IAppointmentRepo
    {
        private readonly DataContext _context;

        public AppointmentRepo(DataContext context)
        {
            _context = context;
        }

        public Task<Appointment> CreateAppointmentAsync(AppointmentUpdateDto newAppointment, CancellationToken token)
        {
            throw new NotImplementedException();   
        }

        public Task<List<Appointment>> GetAllAppointmentsAsync(CancellationToken token)
        {
            return _context.Appointments.ToListAsync(token);
        }
    }
}
