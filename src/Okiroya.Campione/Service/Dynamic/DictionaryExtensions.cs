using System;
using System.Collections.Generic;
using System.Linq;

namespace Okiroya.Campione.Service.Dynamic
{
    /// <summary>
    /// Набор методов-расширений для типа Dictionary
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Взять типизированное значение из словаря по ключу
        /// </summary>
        /// <typeparam name="T">Тип возвращаемого значения</typeparam>
        /// <param name="dictionary">Словарь</param>
        /// <param name="name">Ключ</param>
        /// <returns></returns>
        public static T GetValue<T>(this Dictionary<string, object> dictionary, string name)
        {
            T result = default(T);

            var value = dictionary.FirstOrDefault(p => p.Key.Equals(name, StringComparison.OrdinalIgnoreCase)).Value;

            if ((value != null) && !(value is DBNull))
            {
                result = TypeInterpreter<T>.GetValue(name, value);
            }

            return result;
        }
    }
}
