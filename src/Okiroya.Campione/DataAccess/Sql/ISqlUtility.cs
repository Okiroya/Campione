using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Okiroya.Campione.DataAccess.Sql
{
    public interface ISqlUtility<TResult> where TResult : BaseSqlUtilityResult
    {
        /// <summary>
        /// Выполнить sql хранимую процедуру, изменяющую данные
        /// </summary>
        /// <param name="connectionString">Строка соединения с БД</param>
        /// <param name="commandName">Строка соединения с БД</param>
        /// <param name="parameters">Входные параметры, преобразуемые в параметры хранимой процедуры</param>
        /// <param name="settings">Запрашивать статистику выполнения команды</param>
        /// <returns>Выходные параметры</returns>
        TResult ExecuteCommand(string connectionString, string commandName, IDictionary<string, object> parameters);

        /// <summary>
        /// Выполнить sql хранимую процедуру, изменяющую данные
        /// </summary>
        /// <param name="connectionString">Строка соединения с БД</param>
        /// <param name="commandName">Строка соединения с БД</param>
        /// <param name="parameters">Входные параметры, преобразуемые в параметры хранимой процедуры</param>
        /// <param name="settings">Запрашивать статистику выполнения команды</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Выходные параметры</returns>
        Task<TResult> ExecuteCommandAsync(string connectionString, string commandName, IDictionary<string, object> parameters, CancellationToken cancellationToken);

        /// <summary>
        /// Выполнить sql хранимую процедуру, возвращающую данные
        /// </summary>
        /// <param name="connectionString">Строка соединения с БД</param>
        /// <param name="commandName">Строка соединения с БД</param>
        /// <param name="parameters">Входные параметры, преобразуемые в параметры хранимой процедуры</param>
        /// <param name="settings">Запрашивать статистику выполнения команды</param>
        /// <returns>Набор - заполненный данными и выходные параметры</returns>
        TResult ExecuteQuery(string connectionString, string commandName, IDictionary<string, object> parameters);

        /// <summary>
        /// Выполнить sql хранимую процедуру, возвращающую данные
        /// </summary>
        /// <param name="connectionString">Строка соединения с БД</param>
        /// <param name="commandName">Строка соединения с БД</param>
        /// <param name="parameters">Входные параметры, преобразуемые в параметры хранимой процедуры</param>
        /// <param name="settings">Запрашивать статистику выполнения команды</param>
        /// /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Набор - заполненный данными и выходные параметры</returns>
        Task<TResult> ExecuteQueryAsync(string connectionString, string commandName, IDictionary<string, object> parameters, CancellationToken cancellationToken);
    }
}
