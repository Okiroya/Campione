using System;
using System.Collections.Generic;
using System.Linq;
using Okiroya.Campione.SystemUtility;
using Okiroya.Campione.SystemUtility.DI;
using Okiroya.Campione.Domain;
using Okiroya.Campione.DataAccess;
using Okiroya.Campione.Service.Paging;
using System.Threading;
using System.Threading.Tasks;

namespace Okiroya.Campione.Service
{
    /// <summary>
    /// Фасад для сервиса работы с типизированными сущностями системы
    /// </summary>
    public static class EntityServiceFacade<TResult, TKey>
        where TResult : class, IEntityObject<TKey>
        where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        /// <summary>
        /// Конструктор по-умолчанию
        /// </summary>
        static EntityServiceFacade()
        {
            RegisterDependencyContainer<IEntityService<TResult, TKey>>.SetDefault(new DefaultEntityService<TResult, TKey>());
        }

        /// <summary>
        /// Массовая вставка данных в таблицу
        /// </summary>
        /// <param name="destination">Наименование таблицы для вставки</param>
        /// <param name="data">Данные</param>
        public static void BulkInsert(string destination, IEnumerable<TResult> data)
        {
            ManagedExecutionHelper.TryExecuteAction(RegisterDependencyContainer<IEntityService<TResult, TKey>>.Resolve(destination).BulkInsert, destination, data, "BulkInsert " + destination);
        }

        /// <summary>
        /// Массовая вставка данных в таблицу
        /// </summary>
        /// <param name="destination">Наименование таблицы для вставки</param>
        /// <param name="data">Данные</param>
        /// <param name="cancellationToken">Токен отмены действия</param>
        public static async Task BulkInsertAsync(string destination, IEnumerable<TResult> data, CancellationToken cancellationToken)
        {
            await ManagedExecutionHelper.TryExecuteFunc(RegisterDependencyContainer<IEntityService<TResult, TKey>>.Resolve(destination).BulkInsertAsync, destination, data, cancellationToken, "BulkInsertAsync " + destination);
        }

        /// <summary>
        /// Выполнить команду, которая меняет сущность
        /// </summary>
        /// <param name="commandName">Имя команды</param>
        /// <param name="parameters">Параметры команды</param>
        /// <returns></returns>
        public static IDictionary<string, object> ExecuteCommand(string commandName, IDictionary<string, object> parameters = null)
        {
            return ManagedExecutionHelper.TryExecuteFunc(RegisterDependencyContainer<IEntityService<TResult, TKey>>.Resolve(commandName).ExecuteCommand, commandName, parameters, "ExecuteCommand " + commandName);
        }

        /// <summary>
        /// Выполнить команду, которая меняет сущность
        /// </summary>
        /// <param name="commandName">Имя команды</param>
        /// <param name="parameters">Параметры команды</param>
        /// <param name="cancellationToken">Токен отмены действия</param>
        /// <returns></returns>
        public static async Task<IDictionary<string, object>> ExecuteCommandAsync(string commandName, IDictionary<string, object> parameters, CancellationToken cancellationToken)
        {
            return await ManagedExecutionHelper.TryExecuteFunc(RegisterDependencyContainer<IEntityService<TResult, TKey>>.Resolve(commandName).ExecuteCommandAsync, commandName, parameters, cancellationToken, "ExecuteCommandAsync " + commandName);
        }

        /// <summary>
        /// Выполнить команду, которая возвращает сущность(и)
        /// </summary>
        /// <param name="commandName">Имя команды</param>
        /// <param name="parameters">Параметры команды</param>
        /// <returns></returns>
        public static ServiceResult ExecuteQuery(string commandName, IDictionary<string, object> parameters)
        {
            return ManagedExecutionHelper.TryExecuteFunc(
                (p, q) =>
                {
                    var data = ExecuteTypedQuery(p, q);

                    return new ServiceResult
                    {
                        DataResult = data.DataResult.Select(t => t as IEntityObject<int>),
                        OutParameters = data.OutParameters
                    };
                }, commandName, parameters);
        }

        /// <summary>
        /// Выполнить команду, которая возвращает сущность(и)
        /// </summary>
        /// <param name="commandName">Имя команды</param>
        /// <param name="parameters">Параметры команды</param>
        /// <param name="cancellationToken">Токен отмены действия</param>
        /// <returns></returns>
        public static async Task<ServiceResult> ExecuteQueryAsync(string commandName, IDictionary<string, object> parameters, CancellationToken cancellationToken)
        {
            return await ManagedExecutionHelper.TryExecuteFunc(
                async (p, q) =>
                {
                    var data = await ExecuteTypedQueryAsync(p, q, cancellationToken).ConfigureAwait(false);

                    return new ServiceResult
                    {
                        DataResult = data.DataResult.Select(t => t as IEntityObject<int>),
                        OutParameters = data.OutParameters
                    };
                }, commandName, parameters);
        }

        /// <summary>
        /// Выполнить команду, которая возвращает типизированную(ые) сущность(и)
        /// </summary>
        /// <param name="commandName">Имя команды</param>
        /// <param name="parameters">Параметры команды</param>
        /// <returns></returns>
        public static ServiceResult<TResult, TKey> ExecuteTypedQuery(string commandName, IDictionary<string, object> parameters = null)
        {
            return ManagedExecutionHelper.TryExecuteFunc(RegisterDependencyContainer<IEntityService<TResult, TKey>>.Resolve(commandName).ExecuteTypedQuery, commandName, parameters, "ExecuteTypedQuery " + commandName);
        }

        /// <summary>
        /// Выполнить команду, которая возвращает типизированную(ые) сущность(и)
        /// </summary>
        /// <param name="commandName">Имя команды</param>
        /// <param name="parameters">Параметры команды</param>
        /// <param name="cancellationToken">Токен отмены действия</param>
        /// <returns></returns>
        public static async Task<ServiceResult<TResult, TKey>> ExecuteTypedQueryAsync(string commandName, IDictionary<string, object> parameters, CancellationToken cancellationToken)
        {
            return await ManagedExecutionHelper.TryExecuteFunc(RegisterDependencyContainer<IEntityService<TResult, TKey>>.Resolve(commandName).ExecuteTypedQueryAsync, commandName, parameters, cancellationToken, "ExecuteTypedQueryAsync " + commandName);
        }

        /// <summary>
        /// Вернуть одну типизированную сущность
        /// </summary>
        /// <param name="commandName">Имя команды</param>
        /// <param name="parameters">Параметры команды</param>
        /// <returns></returns>
        public static TResult GetItem(string commandName, IDictionary<string, object> parameters = null)
        {
            var data = ExecuteTypedQuery(commandName, parameters).DataResult;

            return (data != null) && data.Any() ?
                data.FirstOrDefault() :
                default(TResult);
        }

        /// <summary>
        /// Вернуть одну типизированную сущность
        /// </summary>
        /// <param name="commandName">Имя команды</param>
        /// <param name="parameters">Параметры команды</param>
        /// <returns></returns>
        public static async Task<TResult> GetItemAsync(string commandName, IDictionary<string, object> parameters, CancellationToken cancellationToken)
        {
            var data = await ExecuteTypedQueryAsync(commandName, parameters, cancellationToken).ConfigureAwait(false);

            return (data.DataResult != null) && data.DataResult.Any() ?
                data.DataResult.FirstOrDefault() :
                default(TResult);
        }

        /// <summary>
        /// Выполнить команду, которая возвращает коллекцию сущностей для постраничного доступа
        /// </summary>
        /// <param name="commandName">Имя команды</param>
        /// <param name="page">Номер страницы</param>
        /// <param name="pageSize">Размер страницы</param>
        /// <param name="sortBy">Ключ сортировки</param>
        /// <param name="parameters">Параметры команды</param>
        /// <param name="paged">Флаг добавления параметров постраничного доступа</param>
        /// <param name="sortable">Флаг добавления параметров сортировки</param>
        /// <returns></returns>
        public static PagedCollection<TResult> ExecutePagedQuery(string commandName, int page = 1, int pageSize = 0, string sortBy = null, bool paged = true, bool sortable = true, IDictionary<string, object> parameters = null)
        {
            var isAscendingSort = !(sortBy ?? string.Empty).EndsWith(ParametersExtensions.RevParamPostfixName, StringComparison.OrdinalIgnoreCase);

            sortBy = (sortBy ?? string.Empty).Replace(ParametersExtensions.RevParamPostfixName, string.Empty);

            pageSize = pageSize > 0 ? pageSize : ParametersExtensions.DefaultPageSize;

            parameters = parameters ?? new Dictionary<string, object>();

            var executedParameters = parameters;

            if (paged)
            {
                executedParameters = executedParameters.AddPagedParameters(page, pageSize);
            }
            if (sortable)
            {
                executedParameters = executedParameters.AddSortedParameters(sortBy, isAscendingSort);
            }

            var data = ExecuteTypedQuery(commandName, executedParameters);

            var result = new PagedCollection<TResult>(data.DataResult)
            {
                PageIndex = page,
                PageSize = pageSize,
                TotalCount = data.OutParameters.GetOutValue<int>(ParametersExtensions.RowCountParamName),
                SortBy = sortBy,
                IsAscendingSort = isAscendingSort,
                InParams = parameters,
                OutParams = data.OutParameters
            };

            return result;
        }    
    }
}
