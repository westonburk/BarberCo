using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarberCo.SharedLibrary.Dtos
{
    /// <summary>
    /// this dto is used as a lite version of the <see cref="BarberCo.SharedLibrary.Models.Barber"/>
    /// without the sensitive data properties
    /// </summary>
    public class BarberDto
    {
        public string Id { get; set; }
        public bool IsAvaliable { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsDeleted { get; set; }

        public static BarberDto Get(Models.Barber barber)
        {
            var dto = new BarberDto()
            {
                Id = barber.Id,
                IsAvaliable = barber.IsAvailable,
                UserName = barber.UserName,
                Email = barber.Email,
                PhoneNumber = barber.PhoneNumber,
                IsDeleted = barber.DeletedOn != null,
            };

            return dto;
        }
    }
}
