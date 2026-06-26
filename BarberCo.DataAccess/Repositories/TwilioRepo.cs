using BarberCo.SharedLibrary.Dtos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BarberCo.DataAccess.Repositories
{
    public class TwilioRepo : ITwilioRepo
    {
        private readonly HttpClient _http;
        private readonly string _fromNumber;
        private readonly string _messagesUrl;

        public TwilioRepo(HttpClient http, IConfiguration config)
        {
            _http = http;
            var accountSid = config["Twilio:AccountSid"]
                ?? throw new InvalidOperationException("Twilio:AccountSid is not configured");
            var authToken = config["Twilio:AuthToken"]
                ?? throw new InvalidOperationException("Twilio:AuthToken is not configured");
            _fromNumber = config["Twilio:FromPhoneNumber"]
                ?? throw new InvalidOperationException("Twilio:FromPhoneNumber is not configured");
            _messagesUrl = $"https://api.twilio.com/2010-04-01/Accounts/{accountSid}/Messages.json";
            var basic = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{accountSid}:{authToken}"));
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basic);
        }

        public async Task SendSMSAsync(TextMessageDto dto)
        {
            var form = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("To", dto.ToPhoneNumber),
                new KeyValuePair<string, string>("From", _fromNumber),
                new KeyValuePair<string, string>("Body", dto.Message),
            });
            var response = await _http.PostAsync(_messagesUrl, form);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException(
                    $"Twilio SMS send failed ({(int)response.StatusCode}): {error}");
            }
        }
    }
}
