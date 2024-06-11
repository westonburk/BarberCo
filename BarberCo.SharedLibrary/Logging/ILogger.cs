using Microsoft.ApplicationInsights.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarberCo.SharedLibrary.Logging
{
    public interface ILogger
    {
        void LogInfo(string message, SeverityLevel level = SeverityLevel.Information);
        void LogException(Exception exception, SeverityLevel level = SeverityLevel.Critical);
        void LogEvent(string eventName, IDictionary<string, string>? properties = null, IDictionary<string, double>? metrics = null);
    }
}
