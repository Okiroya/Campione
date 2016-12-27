using Okiroya.Campione.SystemUtility;
using System;

namespace Okiroya.Campione.Service.Logging
{
    
    public class ErrorLogEntry : LogEntry
    {
        public Exception ExceptionEntry { get; protected set; }

        public ErrorLogEntry(Exception ex)
            : base()
        {
            Guard.ArgumentNotNull(ex);

            ExceptionEntry = ex;

            Message = string.Concat(ex.Message, ": ", ex.StackTrace);
        }

        public ErrorLogEntry(string error)
        {
            Guard.ArgumentNotEmpty(error);

            Message = error;
        }
    }
}
