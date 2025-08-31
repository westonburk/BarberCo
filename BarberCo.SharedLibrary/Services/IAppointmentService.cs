using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarberCo.SharedLibrary.Services
{
    public interface IAppointmentService
    {
        public Task<List<(DateTime value, string display)>> GetValidTimesForDayAsync(DateTime date);
    }
}
