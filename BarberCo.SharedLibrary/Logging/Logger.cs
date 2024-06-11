using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarberCo.SharedLibrary.Logging
{
    public class Logger : ILogger
    {
        private readonly TelemetryClient _client;

        public Logger(TelemetryClient client)
        {
            _client = client;
        }

        public void LogException(Exception exception, SeverityLevel level = SeverityLevel.Critical)
        {
            var telemetry = new ExceptionTelemetry(exception)
            {
                SeverityLevel = level,
            };

            _client.TrackException(telemetry);
        }

        public void LogInfo(string message, SeverityLevel level = SeverityLevel.Information)
        {
            var traceTelemetry = new TraceTelemetry(message)
            {
                SeverityLevel = level,
            };

            _client.TrackTrace(traceTelemetry);
        }

        public void LogEvent(string eventName, IDictionary<string, string>? properties = null, IDictionary<string, double>? metrics = null)
        {
            var eventTelemetry = new EventTelemetry(eventName);

            if (properties != null)
            {
                foreach (var property in properties)
                {
                    eventTelemetry.Properties.Add(property);
                }
            }

            if (metrics != null)
            {
                foreach (var metric in metrics)
                {
                    eventTelemetry.Metrics.Add(metric);
                }
            }

            _client.TrackEvent(eventTelemetry);
        }
    }
}
