using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarberCo.SharedLibrary.Dtos
{
    public class AppointmentUpdateDto
    {
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public DateTime DateTime { get; set; }
        public List<int> ServiceIds { get; set; }
    }
}
