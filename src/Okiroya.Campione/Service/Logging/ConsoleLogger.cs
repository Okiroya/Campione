using System;

namespace Okiroya.Campione.Service.Logging
{
    public sealed class ConsoleLogger : BaseLogger
    {
        public override void Log(LogEntry logEntry)
        {
            Console.WriteLine(logEntry.Message);
        }
    }
}
