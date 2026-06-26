using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarberCo.SharedLibrary.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        /// <summary>
        /// Appointment Date
        /// </summary>
        public DateTime DateTime { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ConfirmedOn { get; set; }
        public bool ConfirmationFailed { get; set; } = false;
        public string? ConfirmationCodeHash { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public List<Service> Services { get; set; } = [];
    }
}
