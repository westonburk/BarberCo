using BarberCo.SharedLibrary.Models;
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
    public class HourService : IHourService
    {
        private readonly ILogger<HourService> _logger;
        private readonly HttpClient _httpClient;

        public HourService(IHttpClientFactory httpClientFactory, ILogger<HourService> logger)
        {
            _httpClient = httpClientFactory.CreateClient("BarberCoAPI");
            _logger = logger;
        }

        public async Task<List<(int sort, Hour hour)>> GetAllHoursAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("hour");
                response.EnsureSuccessStatusCode();
                var hours = await response.Content.ReadFromJsonAsync<List<Hour>>();
                var results = new List<(int sort, Hour hour)>();
                foreach (var hour in hours)
                {
                    var sort = hour.DayOfWeek.ToLower() switch
                    {
                        "monday" => 0,
                        "tuesday" => 1,
                        "wednesday" => 2,
                        "thursday" => 3,
                        "friday" => 4,
                        "saturday" => 5,
                        "sunday" => 6,
                        _ => 7
                    };

                    results.Add((sort, hour));
                }
                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching hours");
                throw;
            }
        }
    }
}
