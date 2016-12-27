using System;
using System.Linq;
using System.Diagnostics;
using System.Globalization;

namespace Okiroya.Campione.Service.Logging
{
    /// <summary>
    /// Сервис логгирования в Trace
    /// </summary>
    public class TraceLogger : BaseLogger
    {
        public override void Log(LogEntry logEntry)
        {
            var entry = logEntry as TraceLogEntry;
            if (entry != null)
            {
                Trace.WriteLine(
                    string.Format(CultureInfo.CurrentCulture, "{0} - вызов метода {1} с параметрами {2}",
                        entry.LogDateTime,
                        entry.MethodName,
                        (entry.MethodParameters != null) && (entry.MethodParameters.Length > 0) ?
                            entry.MethodParameters.Aggregate((working, next) => { return string.Format("{0}, {1}", working, next); }) :
                            "нет параметров"),
                    entry.Category);
            }
        }
    }
}
