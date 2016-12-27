using System;

namespace Okiroya.Campione.SystemUtility.DI
{
    /// <summary>
    /// Исключение, генерируемое DI контейнером
    /// </summary>
    public class RegisterDependencyException : Exception
    {
        /// <summary>
        /// Конструктор по-умолчанию
        /// </summary>
        public RegisterDependencyException()
        { }

        /// <summary>
        /// Конструктор, принимающий сообщение об ошибке
        /// </summary>
        /// <param name="message"></param>
        public RegisterDependencyException(string message)
            : base(message)
        { }

        /// <summary>
        /// Конструктор, принимающий сообщение об ошибке и исключение
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public RegisterDependencyException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
