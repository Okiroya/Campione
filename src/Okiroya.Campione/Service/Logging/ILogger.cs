using System;

namespace Okiroya.Campione.Service.Logging
{
    /// <summary>
    /// Контракт для сервиса логгирования
    /// </summary>
    public interface ILogger : IObserver<LogEntry>
    {
        /// <summary>
        /// Записать сообщение
        /// </summary>
        /// <param name="logEntry"></param>
        void Log(LogEntry logEntry);
    }
}
