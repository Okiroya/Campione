using System;
using System.Collections;

namespace Okiroya.Campione.DataAccess.MsSql
{
    public class MsSqlUtilityStatisticsResult : DataServiceStatistics
    {
        /// <summary>
        /// Возвращает число пакетов TDS, полученных поставщиком от SQL Server после начала использования приложением поставщика и включения статистики
        /// </summary>
        public long BuffersReceived { get; private set; }

        /// <summary>
        /// Возвращает число пакетов TDS, отправленных на SQL Server поставщиком после включения статистики
        /// </summary>
        public long BuffersSent { get; private set; }

        /// <summary>
        /// Возвращает число байтов данных в пакетах TDS, полученных поставщиком от SQL Server после начала использования приложением поставщика и включения статистики
        /// </summary>
        public long BytesReceived { get; private set; }

        /// <summary>
        /// Возвращает число байтов данных, отправленных на SQL Server в пакетах TDS после начала использования приложением поставщика и включения статистики
        /// </summary>
        public long BytesSent { get; private set; }

        /// <summary>
        /// Возвращает число операций открытия курсора в соединении после начала использования приложением поставщика и включения статистики
        /// </summary>
        public long CursorOpens { get; private set; }

        /// <summary>
        /// Возвращает общее число инструкций insert, delete и update, выполненных в составе соединения после начала использования приложением поставщика и включения статистики
        /// </summary>
        public long IduCount { get; private set; }

        /// <summary>
        /// Возвращает общее число строк, затронутых инструкциями insert, delete и update, которые выполнены в составе соединения после начала использования приложением поставщика и включения статистики
        /// </summary>
        public long IduRows { get; private set; }

        /// <summary>
        /// Возвращает число подготовленных команд, выполненных в соединении после начала использования приложением поставщика и включения статистики
        /// </summary>
        public long PreparedExecs { get; private set; }

        /// <summary>
        /// Возвращает число инструкций, подготовленных в соединении после начала использования приложением поставщика и включения статистики
        /// </summary>
        public long Prepares { get; private set; }

        /// <summary>
        /// Возвращает число инструкций select, выполненных в соединении после начала использования приложением поставщика и включения статистики
        /// </summary>
        public long SelectCount { get; private set; }

        /// <summary>
        /// Возвращает число строк, выбранных после начала использования приложением поставщика и включения статистики
        /// </summary>
        public long SelectRows { get; private set; }

        /// <summary>
        /// Возвращает данные о том, сколько раз в соединении были отправлены команды на сервер и получен ответ после начала использования приложением поставщика и включения статистики
        /// </summary>
        public long ServerRoundtrips { get; private set; }

        /// <summary>
        /// Возвращает число результирующих наборов, использованных после начала использования приложением поставщика и включения статистики
        /// </summary>
        public long SumResultSets { get; private set; }

        /// <summary>
        /// Возвращает число пользовательских транзакций, запущенных после начала использования приложением поставщика и включения статистики, с учетом откатов
        /// </summary>
        public long Transactions { get; private set; }

        /// <summary>
        /// Возвращает число неподготовленных инструкций, выполненных в соединении после начала использования приложением поставщика и включения статистики
        /// </summary>
        public long UnpreparedExecs { get; private set; }

        public MsSqlUtilityStatisticsResult(IDictionary statisticsDictionary)
        {
            BuffersReceived = (long)statisticsDictionary["BuffersReceived"];

            BuffersSent = (long)statisticsDictionary["BuffersSent"];

            BytesReceived = (long)statisticsDictionary["BytesReceived"];

            BytesSent = (long)statisticsDictionary["BytesSent"];

            ConnectionTime = (long)statisticsDictionary["ConnectionTime"];

            CursorOpens = (long)statisticsDictionary["CursorOpens"];

            ExecutionTime = (long)statisticsDictionary["ExecutionTime"];

            IduCount = (long)statisticsDictionary["IduCount"];

            IduRows = (long)statisticsDictionary["IduRows"];

            NetworkServerTime = (long)statisticsDictionary["NetworkServerTime"];

            PreparedExecs = (long)statisticsDictionary["PreparedExecs"];

            Prepares = (long)statisticsDictionary["Prepares"];

            SelectCount = (long)statisticsDictionary["SelectCount"];

            SelectRows = (long)statisticsDictionary["SelectRows"];

            ServerRoundtrips = (long)statisticsDictionary["ServerRoundtrips"];

            SumResultSets = (long)statisticsDictionary["SumResultSets"];

            Transactions = (long)statisticsDictionary["Transactions"];

            UnpreparedExecs = (long)statisticsDictionary["UnpreparedExecs"];
        }

        public override string ToString()
        {
            return string.Concat("bytes-sent:", BytesSent, "bytes-received:", BytesReceived, "connection-time:", ConnectionTime, "execution-time:", ExecutionTime);
        }
    }
}
