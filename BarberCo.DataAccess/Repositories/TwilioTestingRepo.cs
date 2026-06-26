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
    /// <summary>
    /// testing only
    /// </summary>
    public class TwilioTestingRepo : ITwilioRepo
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _config;

        public TwilioTestingRepo(HttpClient http, IConfiguration config)
        {
            _http = http;
            _config = config;
        }

        public async Task SendSMSAsync(TextMessageDto dto)
        {
            var token = _config["SMSAlternativeTelegramDevelopmentOnly:token"]
                ?? throw new InvalidOperationException("SMSAlternativeTelegramDevelopmentOnly:token");
            var chatId = _config["SMSAlternativeTelegramDevelopmentOnly:chat_id"]
                ?? throw new InvalidOperationException("SMSAlternativeTelegramDevelopmentOnly:chat_id");

            var url = $"https://api.telegram.org/bot{token}/sendMessage";
            using var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["chat_id"] = chatId,
                ["text"] = dto.Message,
                ["disable_notification"] = "true"
            });

            var response = await _http.PostAsync(url, content);
        }
    }
}
