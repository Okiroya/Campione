using System;

namespace Okiroya.Campione.DataAccess
{
    /// <summary>
    /// Контракт для сервиса сопоставления одного поля данных к другому. Используется 
    /// </summary>
    public interface IDataServiceMapper
    {
        /// <summary>
        /// Сопоставить поле по наименованию
        /// </summary>
        /// <param name="sourceName">Наименование сопоставляемого поля</param>
        /// <returns>Наименование сопоставляющего поля</returns>
        string MapTo(string sourceName);
    }
}
