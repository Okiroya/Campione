using Okiroya.Campione.SystemUtility;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Okiroya.Campione.DataAccess.Sql
{
    public abstract class DirectSqlDataService<TResult> : BaseDataService, IDataService
        where TResult : BaseSqlUtilityResult
    {
        private string _connectionName;
        private ISqlUtility<TResult> _sqlUtility;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="connectionName"></param>
        /// <param name="settings"></param>
        public DirectSqlDataService(string connectionName, ISqlUtility<TResult> sqlUtility)
        {
            Guard.ArgumentNotEmpty(connectionName);
            Guard.ArgumentNotNull(sqlUtility);

            _connectionName = connectionName;

            _sqlUtility = sqlUtility;
        }

        public override DataQueryResult ExecuteCommand(string commandName, IDictionary<string, object> parameters = null)
        {
            var result = _sqlUtility.ExecuteCommand(ResolveConnectionString(DataServiceCommandType.Query), commandName, parameters);

            return new DataQueryResult(result.ReturnedData, result.OutputParameters, null);
        }

        public override async Task<DataQueryResult> ExecuteCommandAsync(string commandName, IDictionary<string, object> parameters, CancellationToken token)
        {
            var result = await _sqlUtility.ExecuteCommandAsync(ResolveConnectionString(DataServiceCommandType.Query), commandName, parameters, token).ConfigureAwait(false);

            return new DataQueryResult(result.ReturnedData, result.OutputParameters, null);
        }

        public override DataQueryResult ExecuteQuery(string commandName, IDictionary<string, object> parameters)
        {
            var result = _sqlUtility.ExecuteQuery(ResolveConnectionString(DataServiceCommandType.Query), commandName, parameters);

            return new DataQueryResult(result.ReturnedData, result.OutputParameters, null);
        }

        public override async Task<DataQueryResult> ExecuteQueryAsync(string commandName, IDictionary<string, object> parameters, CancellationToken token)
        {
            var result = await _sqlUtility.ExecuteQueryAsync(ResolveConnectionString(DataServiceCommandType.Query), commandName, parameters, token).ConfigureAwait(false);

            return new DataQueryResult(result.ReturnedData, result.OutputParameters, null);
        }

        protected override string ResolveConnectionString(DataServiceCommandType commandType)
        {
            return _connectionName;
        }
    }
}
