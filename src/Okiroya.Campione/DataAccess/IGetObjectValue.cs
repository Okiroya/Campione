using System;

namespace Okiroya.Campione.DataAccess
{
    /// <summary>
    /// Контракт для метода получения значения объекта
    /// </summary>
    public interface IGetObjectValue
    {
        /// <summary>
        /// Вернуть требуемое значение представления объекта
        /// </summary>
        /// <returns></returns>
        object GetValue();
    }
}
