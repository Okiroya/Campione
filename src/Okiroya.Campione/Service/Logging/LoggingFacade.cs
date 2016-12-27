using System;
using System.Collections.Concurrent;
using Okiroya.Campione.SystemUtility;
using System.Threading;
using System.Threading.Tasks;

namespace Okiroya.Campione.Service.Logging
{
    /// <summary>
    /// Фасад сервиса логгирования
    /// </summary>
    public class LoggingFacade : IObservable<LogEntry>, IDisposable
    {
        private ConcurrentBag<IObserver<LogEntry>> _observers = new ConcurrentBag<IObserver<LogEntry>>();
        private ConcurrentQueue<LogEntry> _queueLogEntries = new ConcurrentQueue<LogEntry>();
        private AutoResetEvent _queueWaiter;
        private Task _queueRunner;
        private bool _stop;
        private bool _disposed;

        /// <summary>
        /// Включена ли трассировка или нет. По умолчанию трассировка выключена
        /// </summary>
        /// <remarks>
        /// Включенная трассировка замедляет производительность системы
        /// </remarks>
        public bool EnableTrace { get; set; }

        #region Singleton

        private static LoggingFacade _instance = new LoggingFacade();

        /// <summary>
        /// Вернуть текущий экземпляр фасада
        /// </summary>
        /// <returns></returns>
        public static LoggingFacade GetInstance()
        {
            return _instance;
        }

        private LoggingFacade()
        {
            _queueWaiter = new AutoResetEvent(false);

            _queueRunner = Task.Run(() => ProcessLogEntries());
        }

        #endregion

        /// <summary>
        /// Подписать получателя сообщений
        /// </summary>
        /// <param name="observer"></param>
        /// <returns></returns>
        public IDisposable Subscribe(IObserver<LogEntry> observer)
        {
            Guard.ArgumentNotNull(observer);
            
            _observers.Add(observer);

            return null;
        }

        /// <summary>
        /// Записать лог
        /// </summary>
        /// <param name="logEntry"></param>
        public void Log(LogEntry logEntry)
        {
            Guard.ArgumentNotNull(logEntry);

            _queueLogEntries.Enqueue(logEntry);

            _queueWaiter.Set();
        }

        /// <summary>
        /// Записать трасе
        /// </summary>
        /// <param name="logEntry"></param>
        public void LogTrace(TraceLogEntry logEntry)
        {
            if (EnableTrace)
            {
                Log(logEntry);
            }
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            _stop = true;

            if (disposing)
            {
                if (_queueWaiter != null)
                {
                    _queueWaiter.Reset();
                    _queueWaiter.Dispose();
                }

                if (_queueRunner != null)
                {
                    _queueRunner.GetAwaiter().GetResult();
                }
            }

            _disposed = true;
        }

        private void ProcessLogEntries()
        {
            while (true)
            {
                if (_stop)
                {
                    break;
                }

                while (!_queueLogEntries.IsEmpty)
                {
                    LogEntry logEntry = null;

                    if (_queueLogEntries.TryDequeue(out logEntry))
                    {
                        foreach (var observer in _observers)
                        {
                            observer.OnNext(logEntry);
                        }
                    }
                }

                _queueWaiter.WaitOne();
            }
        }
    }
}
