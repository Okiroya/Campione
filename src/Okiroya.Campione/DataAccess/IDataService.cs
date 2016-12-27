using System;
using System.Collections.Generic;
using Okiroya.Campione.SystemUtility;
using System.Threading.Tasks;
using System.Threading;
using System.Data;

namespace Okiroya.Campione.DataAccess
{
    /// <summary>
    /// Контракт CQRS сервиса для доступа к данным
    /// </summary>
    public interface IDataService
    {
        /// <summary>
        /// Выполнить команду, которая меняет данные или состояние
        /// </summary>
        /// <param name="commandName">Имя команды</param>
        /// <param name="parameters">Параметры команды</param>
        /// <returns>Кортеж с набором прочитанных данных, выходных параметров и статистикой</returns>
        DataQueryResult ExecuteCommand(string commandName, IDictionary<string, object> parameters = null);

        /// <summary>
        /// Выполнить команду, которая меняет данные или состояние
        /// </summary>
        /// <param name="commandName">Имя команды</param>
        /// <param name="parameters">Параметры команды</param>
        /// <param name="cancellationToken">Токен отмены действия</param>
        Task<DataQueryResult> ExecuteCommandAsync(string commandName, IDictionary<string, object> parameters, CancellationToken cancellationToken);

        /// <summary>
        /// Выполнить команду, которая читает данные
        /// </summary>
        /// <param name="commandName">Имя команды</param>
        /// <param name="parameters">Параметры команды</param>
        /// <returns>Кортеж с набором прочитанных данных, выходных параметров и статистикой</returns>
        DataQueryResult ExecuteQuery(string commandName, IDictionary<string, object> parameters = null);

        /// <summary>
        /// Выполнить команду, которая читает данные
        /// </summary>
        /// <param name="commandName">Имя команды</param>
        /// <param name="parameters">Параметры команды</param>
        /// <param name="cancellationToken">Токен отмены действия</param>
        /// <returns>Кортеж с набором прочитанных данных, выходных параметров и статистикой</returns>
        Task<DataQueryResult> ExecuteQueryAsync(string commandName, IDictionary<string, object> parameters, CancellationToken cancellationToken);

        /// <summary>
        /// Массовая вставка данных в таблицу
        /// </summary>
        /// <typeparam name="T">Тип данных</typeparam>
        /// <param name="destination">Наименование таблицы для вставки</param>
        /// <param name="table">Данные</param>
        void BulkInsert<T>(string destination, TableValueParameter<T> table);

        /// <summary>
        /// Массовая вставка данных в таблицу
        /// </summary>
        /// <typeparam name="T">Тип данных</typeparam>
        /// <param name="destination">Наименование таблицы для вставки</param>
        /// <param name="table">Данные</param>
        /// <param name="cancellationToken">Токен отмены действия</param>
        Task BulkInsertAsync<T>(string destination, TableValueParameter<T> table, CancellationToken cancellationToken);
    }
}
