using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Okiroya.Campione.Service
{
    /// <summary>
    /// Контракт для сервиса работы с сущностями системы
    /// </summary>
    public interface IEntityService
    {
        /// <summary>
        /// Выполнить команду, которая меняет сущность
        /// </summary>
        /// <param name="commandName">Имя команды</param>
        /// <param name="parameters">Параметры команды</param>
        /// <returns></returns>
        IDictionary<string, object> ExecuteCommand(string commandName, IDictionary<string, object> parameters = null);

        /// <summary>
        /// Выполнить команду, которая меняет меняет сущность
        /// </summary>
        /// <param name="commandName">Имя команды</param>
        /// <param name="parameters">Параметры команды</param>
        /// <param name="cancellationToken">Токен отмены действия</param>
        Task<IDictionary<string, object>> ExecuteCommandAsync(string commandName, IDictionary<string, object> parameters, CancellationToken cancellationToken);

        /// <summary>
        /// Выполнить команду, которая возвращает сущность(и)
        /// </summary>
        /// <param name="commandName">Имя команды</param>
        /// <param name="parameters">Параметры команды</param>
        /// <returns></returns>
        ServiceResult ExecuteQuery(string commandName, IDictionary<string, object> parameters = null);

        /// <summary>
        /// Выполнить команду, которая возвращает сущность(и)
        /// </summary>
        /// <param name="commandName">Имя команды</param>
        /// <param name="parameters">Параметры команды</param>
        /// <param name="cancellationToken">Токен отмены действия</param>
        /// <returns></returns>
        Task<ServiceResult> ExecuteQueryAsync(string commandName, IDictionary<string, object> parameters, CancellationToken cancellationToken);
    }
}
