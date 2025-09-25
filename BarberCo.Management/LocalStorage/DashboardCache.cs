using BarberCo.SharedLibrary.Models;

namespace BarberCo.Management.LocalStorage
{
    public class DashboardCache
    {
        public List<Appointment> Appointments { get; set; }
        public DateTime Date { get; set; }
    }
}
