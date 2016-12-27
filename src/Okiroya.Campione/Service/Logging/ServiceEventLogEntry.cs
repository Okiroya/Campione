using System;

namespace Okiroya.Campione.Service.Logging
{
    
    public class ServiceEventLogEntry : LogEntry
    {
        public string EventType { get; set; }

        public ServiceEventLogEntry()
            : base()
        { }
    }
}
