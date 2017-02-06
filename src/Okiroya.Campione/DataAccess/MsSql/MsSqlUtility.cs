using Okiroya.Campione.DataAccess.Sql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Reflection;
using System.Xml;

namespace Okiroya.Campione.DataAccess.MsSql
{
    /// <summary>
    /// Утилита выполнения sql команд
    /// </summary>
    public class MsSqlUtility : BaseSqlUtility<SqlConnection, SqlCommand, SqlParameter, SqlDbType, MsSqlUtilityResult>
    {
        /// <summary>
        /// Задание соотношения CLR типов и SQL типов
        /// </summary>
        public MsSqlUtility()
        {
            TypeMap[typeof(byte).GetTypeInfo()] = SqlDbType.TinyInt;
            TypeMap[typeof(byte?).GetTypeInfo()] = SqlDbType.TinyInt;

            TypeMap[typeof(short).GetTypeInfo()] = SqlDbType.SmallInt;
            TypeMap[typeof(short?).GetTypeInfo()] = SqlDbType.SmallInt;

            TypeMap[typeof(int).GetTypeInfo()] = SqlDbType.Int;
            TypeMap[typeof(int?).GetTypeInfo()] = SqlDbType.Int;

            TypeMap[typeof(long).GetTypeInfo()] = SqlDbType.BigInt;
            TypeMap[typeof(long?).GetTypeInfo()] = SqlDbType.BigInt;

            TypeMap[typeof(float).GetTypeInfo()] = SqlDbType.Real;
            TypeMap[typeof(float?).GetTypeInfo()] = SqlDbType.Real;

            TypeMap[typeof(double).GetTypeInfo()] = SqlDbType.Float;
            TypeMap[typeof(double?).GetTypeInfo()] = SqlDbType.Float;

            TypeMap[typeof(decimal).GetTypeInfo()] = SqlDbType.Decimal;
            TypeMap[typeof(decimal?).GetTypeInfo()] = SqlDbType.Decimal;

            TypeMap[typeof(byte[]).GetTypeInfo()] = SqlDbType.Image;

            TypeMap[typeof(bool).GetTypeInfo()] = SqlDbType.Bit;
            TypeMap[typeof(bool?).GetTypeInfo()] = SqlDbType.Bit;

            TypeMap[typeof(string).GetTypeInfo()] = SqlDbType.NVarChar;

            TypeMap[typeof(DateTime).GetTypeInfo()] = SqlDbType.DateTime;
            TypeMap[typeof(DateTime?).GetTypeInfo()] = SqlDbType.DateTime;

            TypeMap[typeof(TimeSpan).GetTypeInfo()] = SqlDbType.Time;
            TypeMap[typeof(TimeSpan?).GetTypeInfo()] = SqlDbType.Time;

            TypeMap[typeof(Guid).GetTypeInfo()] = SqlDbType.UniqueIdentifier;
            TypeMap[typeof(Guid?).GetTypeInfo()] = SqlDbType.UniqueIdentifier;

            TypeMap[typeof(XmlDocument).GetTypeInfo()] = SqlDbType.Xml;
        }

        protected override SqlCommand CreateCommand()
        {
            return new SqlCommand();
        }

        protected override SqlConnection CreateConnection(string connectionName)
        {
            var result = new SqlConnection(connectionName)
            {
                //FireInfoMessageEventOnUserErrors = settings.GetInfoMessages,
                //StatisticsEnabled = settings.RetrieveStatistics
            };

            var infoMessageResult = new List<MsSqlUtilityInfoMessageResult>();

            result.InfoMessage += (sender, e) =>
            {
                for (int i = 0; i < e.Errors.Count; i++)
                {
                    infoMessageResult.Add(new MsSqlUtilityInfoMessageResult(result.Database, e.Errors[i]));
                }
            };

            return result;
        }

        protected override SqlParameter CreateParameter(SqlDbType dbType)
        {
            var result = new SqlParameter
            {
                SqlDbType = dbType
            };

            if (dbType == SqlDbType.NVarChar)
            {
                result.Size = -1;
            }

            return result;
        }

        protected override object GetValue(object val)
        {
            var result = base.GetValue(val);

            if (val is XmlDocument)
            {
                result = new SqlXml(XmlReader.Create(new StringReader(((XmlDocument)val).InnerXml)));
            }

            return result;
        }

        protected override MsSqlUtilityResult CreateResult(ICollection<DataItem> returnedData, IDictionary<string, object> outputParameters)
        {
            return new MsSqlUtilityResult(returnedData, outputParameters, null, null);
        }

        protected override void AfterConnect(SqlConnection connection)
        {
            //if (settings.RetrieveStatistics)
            //{
            //    connection.ResetStatistics();
            //}

            //if (settings.GetInfoMessages)
            //{
            //    result.InfoMessage -=
            //}
        }

        protected override void TakeConnectionData(SqlConnection connection)
        {
            //if (settings.RetrieveStatistics)
            //{
            //    var statisticsResult = new MsSqlUtilityStatisticsResult(connection.RetrieveStatistics());
            //}
        }
    }
}
