using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarberCo.SharedLibrary.Models
{
    public class Barber : IdentityUser
    {
        public bool IsAvailable { get; set; }
    }
}
