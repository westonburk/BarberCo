using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarberCo.SharedLibrary.Dtos
{
    public class TokenDto
    {
        public string Token { get; set; }
        public BarberDto Barber { get; set; }
        public List<string> Roles { get; set; }
        public DateTime Issued { get; set; }
        public DateTime Expires { get; set; }
    }
}
