using BarberCo.SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarberCo.SharedLibrary.Dtos
{
    public class BarberRegistrationResultDto
    {
        public Barber? BarberFull { get; set; }
        public BarberDto? BarberDto { get; set; }
        public bool Successful { get; set; }
        public string? Errors { get; set; }
    }
}
