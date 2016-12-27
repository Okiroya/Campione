using System;

namespace Okiroya.Campione.Service
{
    /// <summary>
    /// Исключение, генерируемое сервисом
    /// </summary>
    public class EntityServiceException : Exception
    {
        /// <summary>
        /// Код возникшей ошибки
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// Наименование команды, в которой возникла ошибка
        /// </summary>
        public string CommandName { get; set; }
        
        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        public override string Message
        {
            get
            {
                return !string.IsNullOrWhiteSpace(CommandName) ?
                    string.Concat("Возникла ошибка при выполнении команды ", CommandName, ": ", base.Message) :
                    base.Message;
            }
        }

        /// <summary>
        /// Конструктор по-умолчанию
        /// </summary>
        public EntityServiceException()
            : base()
        { }

        /// <summary>
        /// Конструктор, принимающий сообщение об ошибке
        /// </summary>
        /// <param name="message"></param>
        public EntityServiceException(string message)
            : base(message)
        { }

        /// <summary>
        /// Конструктор, принимающий сообщение об ошибке и исключение
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public EntityServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
