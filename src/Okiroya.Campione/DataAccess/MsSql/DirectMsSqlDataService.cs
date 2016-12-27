using Okiroya.Campione.DataAccess.Sql;
using Okiroya.Campione.SystemUtility;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Okiroya.Campione.DataAccess.MsSql
{
    /// <summary>
    /// Сервис доступа к sql серверу
    /// </summary>
    public abstract class DirectMsSqlDataService : DirectSqlDataService<MsSqlUtilityResult>
    {
        public MsSqlUtilitySettings Settings { get; protected set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="connectionName"></param>
        /// <param name="settings"></param>
        public DirectMsSqlDataService(string connectionName, MsSqlUtilitySettings settings = null)
            : base(connectionName, new MsSqlUtility())
        {
            Guard.ArgumentNotEmpty(connectionName);

            Settings = settings ?? MsSqlUtilitySettings.Default;
        }

        public override void BulkInsert<T>(string destination, TableValueParameter<T> table)
        {
            MsSqlUtilityBulkCopy<T>.WriteToServer(ResolveConnectionString(DataServiceCommandType.Command), destination, table);
        }

        public override async Task BulkInsertAsync<T>(string destination, TableValueParameter<T> table, CancellationToken cancellationToken)
        {
            await MsSqlUtilityBulkCopy<T>.WriteToServerAsync(ResolveConnectionString(DataServiceCommandType.Command), destination, table, cancellationToken).ConfigureAwait(false);
        }
    }
}
