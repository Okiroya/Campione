using System;
using Okiroya.Campione.SystemUtility;

namespace Okiroya.Campione.Service.Logging
{
    /// <summary>
    /// Базовая реализация сервиса логгирования
    /// </summary>
    public abstract class BaseLogger : ILogger
    {
        #region ILogger

        /// <summary>
        /// Записать сообщение
        /// </summary>
        /// <param name="logEntry"></param>
        public abstract void Log(LogEntry logEntry);

        /// <summary>
        /// 
        /// </summary>
        public void OnCompleted()
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="error"></param>
        public void OnError(Exception error)
        {
            //TO DO: implement turn off logger for some period
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void OnNext(LogEntry value)
        {
            Guard.ArgumentNotNull(value);

            Log(value);
        }

        #endregion    
    }
}
