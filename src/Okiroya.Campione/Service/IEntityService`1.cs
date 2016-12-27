using Okiroya.Campione.Domain;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Okiroya.Campione.Service
{
    /// <summary>
    /// Контракт для сервиса работы с типизированными сущностями системы
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface IEntityService<TResult, TKey> : IEntityService
        where TResult : class, IEntityObject<TKey>
        where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        /// <summary>
        /// Выполнить команду, которая возвращает типизированную(ые) сущность(и)
        /// </summary>
        /// <param name="commandName">Имя команды</param>
        /// <param name="parameters">Параметры команды</param>        
        /// <returns></returns>
        ServiceResult<TResult, TKey> ExecuteTypedQuery(string commandName, IDictionary<string, object> parameters);

        /// <summary>
        /// Выполнить команду, которая возвращает типизированную(ые) сущность(и)
        /// </summary>
        /// <param name="commandName">Имя команды</param>
        /// <param name="parameters">Параметры команды</param>
        /// /// <param name="cancellationToken">Токен отмены действия</param>
        /// <returns></returns>
        Task<ServiceResult<TResult, TKey>> ExecuteTypedQueryAsync(string commandName, IDictionary<string, object> parameters, CancellationToken cancellationToken);
        
        /// <summary>
        /// Массовая вставка данных в таблицу
        /// </summary>
        /// <param name="destination">Наименование таблицы для вставки</param>
        /// <param name="data">Данные</param>
        void BulkInsert(string destination, IEnumerable<TResult> data);

        /// <summary>
        /// Массовая вставка данных в таблицу
        /// </summary>
        /// <param name="destination">Наименование таблицы для вставки</param>
        /// <param name="data">Данные</param>
        /// <param name="cancellationToken">Токен отмены действия</param>
        Task BulkInsertAsync(string destination, IEnumerable<TResult> data, CancellationToken cancellationToken);
    }
}
