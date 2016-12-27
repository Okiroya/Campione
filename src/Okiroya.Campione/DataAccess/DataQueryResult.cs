using System;
using System.Collections.Generic;

namespace Okiroya.Campione.DataAccess
{
    /// <summary>
    /// Кортеж с набором прочитанных данных, выходных параметров и статистики. Используется в CQRS сервисах доступа к данным
    /// </summary>
    
    public class DataQueryResult : Tuple<IEnumerable<DataItem>, IDictionary<string, object>, DataServiceStatistics>
    {
        /// <summary>
        /// Конструирование кортежа
        /// </summary>
        /// <param name="data"></param>
        /// <param name="outParameters"></param>
        /// <param name="statistics"></param>
        public DataQueryResult(IEnumerable<DataItem> data, IDictionary<string, object> outParameters, DataServiceStatistics statistics)
            : base(data, outParameters, statistics)
        { }

        /// <summary>
        /// Набор мета данных
        /// </summary>
        public IEnumerable<DataItem> Data
        {
            get
            {
                return Item1;
            }
        }

        /// <summary>
        /// Возвращаемые параметры
        /// </summary>
        public IDictionary<string, object> OutParameters
        {
            get
            {
                return Item2;
            }
        }

        /// <summary>
        /// Статистика
        /// </summary>
        public DataServiceStatistics Statistics
        {
            get
            {
                return Item3;
            }
        }
    }
}
