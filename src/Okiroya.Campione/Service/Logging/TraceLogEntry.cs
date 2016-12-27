using System;

namespace Okiroya.Campione.Service.Logging
{
    /// <summary>
    /// Запись в trace
    /// </summary>
    
    public class TraceLogEntry : LogEntry
    {
        /// <summary>
        /// Дата и время события
        /// </summary>
        public DateTime LogDateTime { get; protected set; }

        /// <summary>
        /// Категория
        /// </summary>
        public string Category { get; set; }
        
        /// <summary>
        /// Наименование метода
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// Параметры метода
        /// </summary>
        public string[] MethodParameters { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public TraceLogEntry()
            : base()
        {
            LogDateTime = DateTime.Now;
        }
    }
}
