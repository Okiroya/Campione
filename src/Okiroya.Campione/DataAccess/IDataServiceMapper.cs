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
        /// <param name="sourceName">Наименование сопоставляемого поля - доменная сущность</param>
        /// <returns>Наименование сопоставляющего поля - сущность источника данных</returns>
        string MapFromDomainNameToRepositoryOne(string sourceName);
    }
}
