using System;
using System.Collections.Generic;
using Okiroya.Campione.SystemUtility.DI;
using Okiroya.Campione.SystemUtility.Extensions;
using Okiroya.Campione.Domain;
using Okiroya.Campione.DataAccess;
using Okiroya.Campione.Service.Dynamic;
using Okiroya.Campione.Service.Cache;
using System.Threading;
using System.Threading.Tasks;

namespace Okiroya.Campione.Service
{
    /// <summary>
    /// Базовая реализация сервиса работы с типизированными сущностями
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public abstract class BaseEntityService<TResult, TKey> : BaseEntityService, IEntityService<TResult, TKey>
        where TResult : class, IEntityObject<TKey>
        where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        /// <summary>
        /// Массовая вставка данных в таблицу
        /// </summary>
        /// <param name="destination">Наименование таблицы для вставки</param>
        /// <param name="data">Данные</param>
        public void BulkInsert(string destination, IEnumerable<TResult> data)
        {
            RegisterDependencyContainer<IDataService>.Resolve(destination).BulkInsert(
                destination: destination,
                table: RegisterDependencyContainer<TableValueParameter<TResult>>.Resolve(destination).AddData(data));
        }

        /// <summary>
        /// Массовая вставка данных в таблицу
        /// </summary>
        /// <param name="destination">Наименование таблицы для вставки</param>
        /// <param name="data">Данные</param>
        /// <param name="cancellationToken">Токен отмены действия</param>
        public async Task BulkInsertAsync(string destination, IEnumerable<TResult> data, CancellationToken cancellationToken)
        {
            await RegisterDependencyContainer<IDataService>.Resolve(destination).BulkInsertAsync(
                    destination: destination,
                    table: RegisterDependencyContainer<TableValueParameter<TResult>>.Resolve(destination).AddData(data),
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Выполнить команду, которая возвращает типизированную(ые) сущность(и)
        /// </summary>
        /// <param name="commandName">Имя команды</param>
        /// <param name="parameters">Параметры команды</param>
        /// <returns></returns>
        public virtual ServiceResult<TResult, TKey> ExecuteTypedQuery(string commandName, IDictionary<string, object> parameters)
        {
            var data = CacheFacade.AddOrGetExisting(
                commandName,
                parameters,
                () =>
                {
                    return RegisterDependencyContainer<IDataService>.Resolve(commandName).ExecuteQuery(commandName, parameters);
                });

            return new ServiceResult<TResult, TKey>
            {
                DataResult = ConvertDataItems(commandName, parameters, data.Item1),
                OutParameters = data.Item2
            };
        }

        /// <summary>
        /// Выполнить команду, которая возвращает типизированную(ые) сущность(и)
        /// </summary>
        /// <param name="commandName">Имя команды</param>
        /// <param name="parameters">Параметры команды</param>
        /// /// <param name="cancellationToken">Токен отмены действия</param>
        public async Task<ServiceResult<TResult, TKey>> ExecuteTypedQueryAsync(string commandName, IDictionary<string, object> parameters, CancellationToken cancellationToken)
        {
            var result = await RegisterDependencyContainer<IDataService>
                .Resolve(commandName)
                .ExecuteQueryAsync(commandName, parameters, cancellationToken)
                .ConfigureAwait(false);

            var data = CacheFacade.AddOrGetExisting(
                commandName,
                parameters,
                () =>
                {
                    return result;
                });

            return new ServiceResult<TResult, TKey>
            {
                DataResult = ConvertDataItems(commandName, parameters, data.Item1),
                OutParameters = data.Item2
            };
        }

        /// <summary>
        /// Выполнить команду, которая возвращает типизированную(ые) сущность(и)
        /// </summary>
        /// <param name="commandName">Имя команды</param>
        /// <param name="parameters">Параметры команды</param>
        /// <returns></returns>
        public virtual ServiceResult<TResult, TKey> ExecuteTypedQuery(string commandName, params string[] parameters)
        {
            return ExecuteTypedQuery(commandName, MakeParameterDictionary(parameters));
        }

        /// <summary>
        /// Преобразование набора данных от data service к сущности
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        protected virtual IEnumerable<TResult> ConvertDataItems(string commandName, IDictionary<string, object> parameters, IEnumerable<DataItem> items)
        {
            return items.EnumerateDataItems(commandName, parameters, ConvertToTypedEntity);
        }

        /// <summary>
        /// Преобразование набора мета информации к сущности
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="dataItem"></param>
        /// <returns></returns>
        protected virtual TResult ConvertToTypedEntity(string commandName, IDictionary<string, object> parameters, DataItem dataItem)
        {
            return EntityObjectGenerator<TKey>.CreateEntityObjectFromMeta<TResult>(dataItem.Items);
        }
    }
}
