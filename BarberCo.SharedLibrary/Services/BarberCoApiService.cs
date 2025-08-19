using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BarberCo.SharedLibrary.Services
{
    public class BarberCoApiService : IBarberCoApiService
    {
        private readonly HttpClient _httpClient;

        public BarberCoApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("BarberCoAPI");
        }

        public async Task<bool> DeleteAsync(string endpoint, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.DeleteAsync(endpoint, cancellationToken);
            response.EnsureSuccessStatusCode();
            return response.StatusCode == System.Net.HttpStatusCode.OK;
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest data, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.PostAsJsonAsync(endpoint, data, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken);
        }

        public async Task<T> GetAsync<T>(string endpoint, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetAsync(endpoint, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken);
        }

        public async Task<TResponse> PutAsync<TRequest, TResponse>(string endpoint, TRequest data, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.PutAsJsonAsync(endpoint, data, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken);
        }
    }
}
