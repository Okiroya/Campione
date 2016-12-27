using System;
using System.Collections.Generic;
using System.Linq;
using Okiroya.Campione.SystemUtility.DI;
using Okiroya.Campione.Domain;
using Okiroya.Campione.DataAccess;
using Okiroya.Campione.Service.Paging;
using System.Threading;
using System.Threading.Tasks;

namespace Okiroya.Campione.Service
{
    /// <summary>
    /// Фасад для сервиса работы с сущностями системы
    /// </summary>
    public static class EntityServiceFacade
    {
        /// <summary>
        /// Конструктор по-умолчанию
        /// </summary>
        static EntityServiceFacade()
        {
            RegisterDependencyContainer<IEntityService>.SetDefault(new DefaultEntityService());
        }
        
        /// <summary>
        /// Выполнить команду, которая меняет сущность
        /// </summary>
        /// <param name="commandName">Имя команды</param>
        /// <param name="parameters">Параметры команды</param>
        /// <returns></returns>
        public static IDictionary<string, object> ExecuteCommand(string commandName, IDictionary<string, object> parameters = null)
        {
            return ManagedExecutionHelper.TryExecuteFunc(RegisterDependencyContainer<IEntityService>.Resolve(commandName).ExecuteCommand, commandName, parameters, "ExecuteCommand " + commandName);
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
            return await ManagedExecutionHelper.TryExecuteFunc(RegisterDependencyContainer<IEntityService>.Resolve(commandName).ExecuteCommandAsync, commandName, parameters, cancellationToken, "ExecuteCommandAsync " + commandName);
        }

        /// <summary>
        /// Выполнить команду, которая возвращает сущность(и)
        /// </summary>
        /// <param name="commandName">Имя команды</param>
        /// <param name="parameters">Параметры команды</param>
        /// <returns></returns>
        public static ServiceResult ExecuteQuery(string commandName, IDictionary<string, object> parameters = null)
        {
            return ManagedExecutionHelper.TryExecuteFunc(RegisterDependencyContainer<IEntityService>.Resolve(commandName).ExecuteQuery, commandName, parameters, "ExecuteQuery " + commandName);
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
            return await ManagedExecutionHelper.TryExecuteFunc(RegisterDependencyContainer<IEntityService>.Resolve(commandName).ExecuteQueryAsync, commandName, parameters, cancellationToken, "ExecuteQueryAsync " + commandName);
        }

        /// <summary>
        /// Вернуть одну сущность
        /// </summary>
        /// <param name="commandName">Имя команды</param>
        /// <param name="parameters">Параметры команды</param>
        /// <returns></returns>
        public static IEntityObject<int> GetItem(string commandName, IDictionary<string, object> parameters = null)
        {
            var data = ExecuteQuery(commandName, parameters).DataResult;

            return (data != null) && data.Any() ?
                data.FirstOrDefault() :
                null;
        }

        /// <summary>
        /// Вернуть одну сущность
        /// </summary>
        /// <param name="commandName">Имя команды</param>
        /// <param name="parameters">Параметры команды</param>
        /// <returns></returns>
        public static async Task<IEntityObject<int>> GetItemAsync(string commandName, IDictionary<string, object> parameters, CancellationToken cancellationToken)
        {
            var data = await ExecuteQueryAsync(commandName, parameters, cancellationToken).ConfigureAwait(false);

            return (data.DataResult != null) && data.DataResult.Any() ?
                data.DataResult.FirstOrDefault() :
                null;
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
        public static PagedCollection<IEntityObject<int>> ExecutePagedQuery(string commandName, int page = 1, int pageSize = 0, string sortBy = null, bool paged = true, bool sortable = true, IDictionary<string, object> parameters = null)
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

            var data = ExecuteQuery(commandName, executedParameters);

            var result = new PagedCollection<IEntityObject<int>>(data.DataResult)
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
