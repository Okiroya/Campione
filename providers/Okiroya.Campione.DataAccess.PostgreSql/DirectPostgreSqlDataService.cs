using Okiroya.Campione.DataAccess.Sql;
using System;

namespace Okiroya.Campione.DataAccess.PostgreSql
{
    public abstract class DirectPostgreSqlDataService : DirectSqlDataService<BaseSqlUtilityResult>
    {
        private string _connectionName;

        private PostgreSqlUtility _sqlUtility = new PostgreSqlUtility();

        public DirectPostgreSqlDataService(string connectionName)
            : base(connectionName, new PostgreSqlUtility())
        { }
    }
}
