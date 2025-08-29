using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BarberCo.SharedLibrary.Models
{
    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        [JsonIgnore]
        public List<Appointment> Appointments { get; } = [];
    }
}
