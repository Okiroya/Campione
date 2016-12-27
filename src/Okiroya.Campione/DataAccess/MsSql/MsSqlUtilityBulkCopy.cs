using Okiroya.Campione.SystemUtility;
using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace Okiroya.Campione.DataAccess.MsSql
{
    public static class MsSqlUtilityBulkCopy<T>
    {
        public static void WriteToServer(string connectionString, string destination, TableValueParameter<T> table)
        {
            Guard.ArgumentNotEmpty(connectionString);
            Guard.ArgumentNotEmpty(destination);
            Guard.ArgumentNotNull(table);

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var loader = CreateSqlBulkCopy(connection, destination, table))
                {
                    loader.WriteToServer(table);
                }
            }
        }

        public static async Task WriteToServerAsync(string connectionString, string destination, TableValueParameter<T> table, CancellationToken cancellationToken)
        {
            Guard.ArgumentNotEmpty(connectionString);
            Guard.ArgumentNotEmpty(destination);
            Guard.ArgumentNotNull(table);

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync(cancellationToken);

                using (var loader = CreateSqlBulkCopy(connection, destination, table))
                {
                    await loader.WriteToServerAsync(table, cancellationToken).ConfigureAwait(false);
                }
            }
        }

        private static SqlBulkCopy CreateSqlBulkCopy(SqlConnection connection, string destination, TableValueParameter<T> table)
        {
            var result = new SqlBulkCopy(connection);
            result.BulkCopyTimeout = connection.ConnectionTimeout;

            result.DestinationTableName = destination;
            result.BatchSize = table.FieldCount;

            if (table.HasColumnMapping)
            {
                var enumerator = table.GetColumnMappingEnumerator();
                while (enumerator.MoveNext())
                {
                    result.ColumnMappings.Add(enumerator.Current.Key, enumerator.Current.Value);
                }
            }

            return result;
        }
    }
}
