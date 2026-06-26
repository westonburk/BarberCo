using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarberCo.SharedLibrary.Dtos
{
    public class AppointmentConfirmationDto
    {
        public int AppointmentId { get; set; }
        public string ConfirmationCode { get; set; }
    }
}
