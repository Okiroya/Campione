using System;

namespace Okiroya.Campione.SystemUtility
{
    /// <summary>
    /// Утилита для проверки входных данных
    /// </summary>
    public static class Guard
    {
        /// <summary>
        /// Проверка, что входной параметр не null или default
        /// </summary>
        /// <typeparam name="T">Тип входного параметра</typeparam>
        /// <param name="argument">Входной параметр</param>
        /// <exception cref="ArgumentNullException">Генерируемый тип исключения</exception>
        public static void ArgumentNotNull<T>(T argument)
        {
            if (Equals(argument, default(T)))
                throw new ArgumentNullException(
                    nameof(argument),
                    string.Concat(nameof(argument), " не задан"));
        }

        /// <summary>
        /// Проверка, что входная строка не пустая
        /// </summary>
        /// <param name="argument">Входная строка</param>
        /// <param name="message">Сообщение, передаваемое в исключение, если проверка не прошла</param>
        /// <exception cref="ArgumentNullException">Генерируемый тип исключения</exception>
        /// <exception cref="ArgumentException">Генерируемый тип исключения</exception>
        public static void ArgumentNotEmpty(string argument, string message = null)
        {
            if (string.IsNullOrWhiteSpace(argument))
            {
                if (argument == null)
                {
                    throw new ArgumentNullException(!string.IsNullOrWhiteSpace(message) ? message : string.Concat(nameof(argument), " is null"));
                }
                else
                {
                    throw new ArgumentException(!string.IsNullOrWhiteSpace(message) ? message : string.Concat(nameof(argument), " пустой"));
                }
            }
        }
    }
}
