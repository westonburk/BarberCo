using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarberCo.SharedLibrary.Services
{
    public interface IBarberCoApiService
    {
        Task<T> GetAsync<T>(string endpoint, CancellationToken cancellationToken = default);
        Task<TResponse> PutAsync<TRequest, TResponse>(string endpoint, TRequest data, CancellationToken cancellationToken = default);
        Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest data, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(string endpoint, CancellationToken cancellationToken = default);
    } 
}
