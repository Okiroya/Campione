using System;
using System.Collections.Generic;
using System.Linq;
using Okiroya.Campione.SystemUtility;

namespace Okiroya.Campione
{
    /// <summary>
    /// Методы-расширения, предоставляющие удобный API
    /// </summary>
    public static class FluentExtensions
    {
        /// <summary>
        /// Перечисление имеет значения
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool HasItems<T>(this IEnumerable<T> source)
        {
            return (source != null) && source.Any();
        }

        /// <summary>
        /// Предоставление методов и свойств сущности для организации fluent интерфейса
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="setter"></param>
        /// <returns></returns>
        public static T FluentIt<T>(this T source, Action<T> setter)
        {
            Guard.ArgumentNotNull(source);

            setter?.Invoke(source);

            return source;
        }
    }
}
