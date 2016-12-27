using System;
using System.Collections.Generic;

namespace Okiroya.Campione.Service.Paging
{
    /// <summary>
    /// Коллекция, которую можно читать постранично
    /// </summary>
    public class PagedCollection : IPagedCollection
    {
        /// <summary>
        /// Индекс страницы
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// Размер страницы - количество элементов на странице
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Общее количество элементов в коллекции
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Наименования параметра сортировки
        /// </summary>
        public string SortBy { get; set; }

        /// <summary>
        /// Флаг - коллекция отсортирована по возрастанию
        /// </summary>
        public bool IsAscendingSort { get; set; }

        /// <summary>
        /// Входящие параметры
        /// </summary>
        public IDictionary<string, object> InParams { get; set; }

        /// <summary>
        /// Выходные параметры
        /// </summary>
        public IDictionary<string, object> OutParams { get; set; }
    }

}
