using Npgsql;
using NpgsqlTypes;
using Okiroya.Campione.DataAccess.Sql;
using Okiroya.Campione.SystemUtility;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;

namespace Okiroya.Campione.DataAccess.PostgreSql
{
    public class PostgreSqlUtility : BaseSqlUtility<NpgsqlConnection, NpgsqlCommand, NpgsqlParameter, NpgsqlDbType, BaseSqlUtilityResult>
    {
        /// <summary>
        /// Задание соотношения CLR типов и SQL типов
        /// </summary>
        public PostgreSqlUtility()
        {
            TypeMap[typeof(byte).GetTypeInfo()] = NpgsqlDbType.Smallint;
            TypeMap[typeof(byte?).GetTypeInfo()] = NpgsqlDbType.Smallint;

            TypeMap[typeof(int).GetTypeInfo()] = NpgsqlDbType.Integer;
            TypeMap[typeof(int?).GetTypeInfo()] = NpgsqlDbType.Integer;

            TypeMap[typeof(long).GetTypeInfo()] = NpgsqlDbType.Bigint;
            TypeMap[typeof(long?).GetTypeInfo()] = NpgsqlDbType.Bigint;

            TypeMap[typeof(float).GetTypeInfo()] = NpgsqlDbType.Real;
            TypeMap[typeof(float?).GetTypeInfo()] = NpgsqlDbType.Real;

            TypeMap[typeof(double).GetTypeInfo()] = NpgsqlDbType.Double;
            TypeMap[typeof(double?).GetTypeInfo()] = NpgsqlDbType.Double;

            TypeMap[typeof(decimal).GetTypeInfo()] = NpgsqlDbType.Numeric;
            TypeMap[typeof(decimal?).GetTypeInfo()] = NpgsqlDbType.Numeric;

            TypeMap[typeof(byte[]).GetTypeInfo()] = NpgsqlDbType.InternalChar;

            TypeMap[typeof(bool).GetTypeInfo()] = NpgsqlDbType.Boolean;
            TypeMap[typeof(bool?).GetTypeInfo()] = NpgsqlDbType.Boolean;

            TypeMap[typeof(string).GetTypeInfo()] = NpgsqlDbType.Varchar;

            TypeMap[typeof(DateTime).GetTypeInfo()] = NpgsqlDbType.Timestamp;
            TypeMap[typeof(DateTime?).GetTypeInfo()] = NpgsqlDbType.Timestamp;

            TypeMap[typeof(TimeSpan).GetTypeInfo()] = NpgsqlDbType.Time;
            TypeMap[typeof(TimeSpan?).GetTypeInfo()] = NpgsqlDbType.Time;

            TypeMap[typeof(Guid).GetTypeInfo()] = NpgsqlDbType.Uuid;
            TypeMap[typeof(Guid?).GetTypeInfo()] = NpgsqlDbType.Uuid;

            TypeMap[typeof(XmlDocument).GetTypeInfo()] = NpgsqlDbType.Xml;
        }

        protected override NpgsqlConnection CreateConnection(string connectionName)
        {
            Guard.ArgumentNotEmpty(connectionName);

            return new NpgsqlConnection(connectionName);
        }

        protected override NpgsqlCommand CreateCommand()
        {
            return new NpgsqlCommand();
        }

        protected override NpgsqlParameter CreateParameter(NpgsqlDbType dbType)
        {
            return new NpgsqlParameter()
            {
                NpgsqlDbType = dbType
            };
        }

        protected override BaseSqlUtilityResult CreateResult(ICollection<DataItem> returnedData, IDictionary<string, object> outputParameters)
        {
            return new BaseSqlUtilityResult(returnedData, outputParameters);
        }
    }
}
