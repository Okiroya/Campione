using System;
using System.Collections.Generic;

namespace Okiroya.Campione.DataAccess
{
    /// <summary>
    /// Контейнер для прочитанных данных
    /// </summary>
    
    public class DataItem
    {
        /// <summary>
        /// Порядковый номер записи
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Список элементов
        /// </summary>
        public IEnumerable<DataItemElement> Items { get; set; }
    }
}
