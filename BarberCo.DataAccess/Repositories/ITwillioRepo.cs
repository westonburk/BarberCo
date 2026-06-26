using BarberCo.SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarberCo.DataAccess.Repositories
{
    public interface ITwillioRepo
    {
        Task SendSMSAsync(TextMessageDto message);
    }
}
