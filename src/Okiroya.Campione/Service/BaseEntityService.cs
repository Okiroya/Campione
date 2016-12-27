using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Базовая реализация сервиса работы с сущностями
    /// </summary>
    public abstract class BaseEntityService : IEntityService
    {
        /// <summary>
        /// Выполнить команду, которая меняет сущность
        /// </summary>
        /// <param name="commandName">Имя команды</param>
        /// <param name="parameters">Параметры команды</param>
        /// <returns></returns>
        public virtual IDictionary<string, object> ExecuteCommand(string commandName, IDictionary<string, object> parameters = null)
        {
            var result = RegisterDependencyContainer<IDataService>.Resolve(commandName).ExecuteCommand(commandName, parameters).OutParameters;

            var dependencies = DependencyCacheRegister.GetDependencies(commandName);

            if ((dependencies != null) && dependencies.Any())
            {
                CacheFacade.Remove(dependencies);
            }

            return result;
        }

        /// <summary>
        /// Выполнить команду, которая меняет сущность
        /// </summary>
        /// <param name="commandName">Имя команды</param>
        /// <param name="parameters">Параметры команды</param>
        /// <param name="cancellationToken">Токен отмены действия</param>
        /// <returns></returns>
        public async Task<IDictionary<string, object>> ExecuteCommandAsync(string commandName, IDictionary<string, object> parameters, CancellationToken cancellationToken)
        {
            var result = await RegisterDependencyContainer<IDataService>
                .Resolve(commandName)
                .ExecuteCommandAsync(commandName, parameters, cancellationToken)
                .ConfigureAwait(false);

            var dependencies = DependencyCacheRegister.GetDependencies(commandName);

            if ((dependencies != null) && dependencies.Any())
            {
                CacheFacade.Remove(dependencies);
            }

            return result.OutParameters;
        }

        /// <summary>
        /// Выполнить команду, которая возвращает сущность(и)
        /// </summary>
        /// <param name="commandName">Имя команды</param>
        /// <param name="parameters">Параметры команды</param>
        /// <returns></returns>
        public virtual ServiceResult ExecuteQuery(string commandName, IDictionary<string, object> parameters = null)
        {
            var data = CacheFacade.AddOrGetExisting(
                commandName,
                parameters,
                () =>
                {
                    return RegisterDependencyContainer<IDataService>.Resolve(commandName).ExecuteQuery(commandName, parameters);
                });

            return new ServiceResult
            {
                DataResult = data.Item1.EnumerateDataItems(commandName, ConvertToEntity), //TODO: сделать также как и в типизированном сервисе
                OutParameters = data.Item2
            };
        }

        /// <summary>
        /// Выполнить команду, которая возвращает сущность(и)
        /// </summary>
        /// <param name="commandName">Имя команды</param>
        /// <param name="parameters">Параметры команды</param>
        /// <param name="cancellationToken">Токен отмены действия</param>
        /// <returns></returns>
        public async Task<ServiceResult> ExecuteQueryAsync(string commandName, IDictionary<string, object> parameters, CancellationToken cancellationToken)
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

            return new ServiceResult
            {
                DataResult = data.Item1.EnumerateDataItems(commandName, ConvertToEntity), //TODO: сделать также как и в типизированном сервисе
                OutParameters = data.Item2
            };
        }

        /// <summary>
        /// Выполнить команду, которая возвращает сущность(и)
        /// </summary>
        /// <param name="commandName">Имя команды</param>
        /// <param name="parameters">Параметры команды</param>
        /// <returns></returns>
        public virtual ServiceResult ExecuteQuery(string commandName, params string[] parameters)
        {
            return ExecuteQuery(commandName, MakeParameterDictionary(parameters));
        }

        /// <summary>
        /// Преобразование набора мета информации к сущности
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="dataItem"></param>
        /// <returns></returns>
        protected virtual IEntityObject<int> ConvertToEntity(string commandName, DataItem dataItem)
        {
            return EntityObjectGenerator<int>.CreateEntityObjectFromMeta(commandName, dataItem.Items);
        }

        /// <summary>
        /// Преобразование набор параметров в словарь параметров
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected virtual IDictionary<string, object> MakeParameterDictionary(params string[] parameters)
        {
            IDictionary<string, object> result = null;

            if (parameters != null)
            {
                int count = parameters.Length / 2;
                result = new Dictionary<string, object>(count);

                for (int i = 0; i < count; i++)
                {
                    result.Add(parameters[i * 2], parameters[i * 2 + 1]);
                }
            }

            return result;
        }
    }
}
