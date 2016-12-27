using System;
using System.Data.SqlClient;

namespace Okiroya.Campione.DataAccess.MsSql
{
    public class MsSqlUtilityInfoMessageResult
    {
        /// <summary>
        /// Номер ошибки, возращаемый MS SQL
        /// </summary>
        public int Code { get; private set; }

        /// <summary>
        /// Текст внутреннего сообщения
        /// </summary>
        public string InternalMessage { get; private set; }

        /// <summary>
        /// Название stored procedure
        /// </summary>
        public string Procedure { get; private set; }

        /// <summary>
        /// Номер строки с ошибкой
        /// </summary>
        public int LineNumber { get; private set; }

        /// <summary>
        /// Уровень важности ошибки, возвращаемой MS SQL
        /// </summary>
        public byte Severity { get; private set; }

        /// <summary>
        /// Имя сервера MS SQL
        /// </summary>
        public string Server { get; private set; }

        /// <summary>
        /// Имя БД MS SQL
        /// </summary>
        public string Database { get; private set; }

        public MsSqlUtilityInfoMessageResult(string database, SqlError error)
        {
            if (error != null)
            {
                Code = error.Number;

                InternalMessage = error.Message;

                Procedure = error.Procedure;

                LineNumber = error.LineNumber;

                Severity = error.Class;

                Server = error.Server;
            }

            Database = database;
        }
    }
}
