using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Okiroya.Campione.DataAccess
{
    /// <summary>
    /// Базовый сервис работы с данными
    /// </summary>
    public abstract class BaseDataService : IDataService
    {
        /// <summary>
        /// Массовая вставка данных в таблицу
        /// </summary>
        /// <typeparam name="T">Тип данных</typeparam>
        /// <param name="destination">Наименование таблицы для вставки</param>
        /// <param name="table">Данные</param>
        public abstract void BulkInsert<T>(string destination, TableValueParameter<T> table);

        /// <summary>
        /// Массовая вставка данных в таблицу
        /// </summary>
        /// <typeparam name="T">Тип данных</typeparam>
        /// <param name="destination">Наименование таблицы для вставки</param>
        /// <param name="table">Данные</param>
        /// <param name="cancellationToken">Токен отмены</param>
        public abstract Task BulkInsertAsync<T>(string destination, TableValueParameter<T> table, CancellationToken cancellationToken);

        /// <summary>
        /// Выполнить команду, которая меняет данные или состояние
        /// </summary>
        /// <param name="commandName">Имя команды</param>
        /// <param name="parameters">Параметры команды</param>
        /// <returns>Кортеж с набором прочитанных данных, выходных параметров и статистикой</returns>
        public abstract DataQueryResult ExecuteCommand(string commandName, IDictionary<string, object> parameters);

        /// <summary>
        /// Выполнить команду, которая меняет данные или состояние
        /// </summary>
        /// <param name="commandName">Имя команды</param>
        /// <param name="parameters">Параметры команды</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Кортеж с набором прочитанных данных, выходных параметров и статистикой</returns>
        public abstract Task<DataQueryResult> ExecuteCommandAsync(string commandName, IDictionary<string, object> parameters, CancellationToken cancellationToken);

        /// <summary>
        /// Выполнить команду, которая читает данные
        /// </summary>
        /// <param name="commandName">Имя команды</param>
        /// <param name="parameters">Параметры команды</param>
        /// <returns>Кортеж с набором прочитанных данных, выходных параметров и статистикой</returns>
        public abstract DataQueryResult ExecuteQuery(string commandName, IDictionary<string, object> parameters);

        /// <summary>
        /// Выполнить команду, которая читает данные
        /// </summary>
        /// <param name="commandName">Имя команды</param>
        /// <param name="parameters">Параметры команды</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Кортеж с набором прочитанных данных, выходных параметров и статистикой</returns>
        public abstract Task<DataQueryResult> ExecuteQueryAsync(string commandName, IDictionary<string, object> parameters, CancellationToken cancellationToken);

        protected abstract string ResolveConnectionString(DataServiceCommandType commandType);
    }
}
