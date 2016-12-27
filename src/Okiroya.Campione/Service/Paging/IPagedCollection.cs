using System;
using System.Collections.Generic;

namespace Okiroya.Campione.Service.Paging
{
    /// <summary>
    /// Контракт для коллекции, которую можно читать постранично
    /// </summary>
    public interface IPagedCollection
    {
        /// <summary>
        /// Индекс страницы
        /// </summary>
        int PageIndex { get; set; }

        /// <summary>
        /// Размер страницы - количество элементов на странице
        /// </summary>
        int PageSize { get; set; }

        /// <summary>
        /// Общее количество элементов в коллекции
        /// </summary>
        int TotalCount { get; set; }

        /// <summary>
        /// Наименования параметра сортировки
        /// </summary>
        string SortBy { get; set; }

        /// <summary>
        /// Флаг - коллекция отсортирована по возрастанию
        /// </summary>
        bool IsAscendingSort { get; set; }

        /// <summary>
        /// Входящие параметры
        /// </summary>
        IDictionary<string, object> InParams { get; set; }

        /// <summary>
        /// Выходные параметры
        /// </summary>
        IDictionary<string, object> OutParams { get; set; }
    }
}
