using Okiroya.Campione.SystemUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Okiroya.Campione.DataAccess.Sql
{
    public abstract class BaseSqlUtility<TConnection, TCommand, TParameter, TDbType, TResult> : ISqlUtility<TResult>
        where TConnection : DbConnection
        where TCommand : DbCommand
        where TParameter : DbParameter
        where TDbType : struct
        where TResult : BaseSqlUtilityResult
    {
        private Dictionary<TypeInfo, TDbType> _typeMap = new Dictionary<TypeInfo, TDbType>();
        private Dictionary<string, string> _commandTextMap = new Dictionary<string, string>();

        protected Dictionary<TypeInfo, TDbType> TypeMap
        {
            get
            {
                return _typeMap;
            }
        }

        static BaseSqlUtility()
        {
            if (!typeof(TDbType).GetTypeInfo().IsEnum)
            {
                throw new ArgumentException("TDbType должен быть enum");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clrTypeInfo"></param>
        /// <param name="dbType"></param>
        public void AddSqlMapping(TypeInfo clrTypeInfo, TDbType dbType)
        {
            Guard.ArgumentNotNull(clrTypeInfo);
            Guard.ArgumentNotNull(dbType);

            lock (_typeMap)
            {
                if (_typeMap.ContainsKey(clrTypeInfo))
                {
                    throw new NotSupportedException($"Тип {clrTypeInfo.ToString()} уже задан для маппинга. Маппинг {clrTypeInfo.ToString()} в {_typeMap[clrTypeInfo].ToString()}");
                }
                else
                {
                    _typeMap.Add(clrTypeInfo, dbType);
                }
            }
        }

        /// <summary>
        /// Добавить отображение текста команды на наименование команды
        /// </summary>
        /// <remarks>
        /// Используется для выполнения sql команды, заданной текстом (тип CommandType.Text), а не хранимкой
        /// </remarks>
        /// <param name="commandName">Наименование команды</param>
        /// <param name="commandText">Текст команды</param>
        public void AddCommandMapping(string commandName, string commandText)
        {
            Guard.ArgumentNotEmpty(commandName);
            Guard.ArgumentNotEmpty(commandText);

            lock (_commandTextMap)
            {
                if (_commandTextMap.ContainsKey(commandName))
                {
                    _commandTextMap[commandName] = commandText;
                }
                else
                {
                    _commandTextMap.Add(commandName, commandText);
                }
            }
        }

        /// <summary>
        /// Выполнить sql хранимую процедуру, изменяющую данные
        /// </summary>
        /// <param name="connectionString">Строка соединения с БД</param>
        /// <param name="commandName">Строка соединения с БД</param>
        /// <param name="parameters">Входные параметры, преобразуемые в параметры хранимой процедуры</param>
        /// <param name="settings">Запрашивать статистику выполнения команды</param>
        /// <returns>Выходные параметры</returns>
        public TResult ExecuteCommand(string connectionString, string commandName, IDictionary<string, object> parameters)
        {
            return InternalExecuteSqlCommand(connectionString, commandName, parameters != null ? GenerateParameters(parameters) : null, RunExecuteNonQuery);
        }

        public async Task<TResult> ExecuteCommandAsync(string connectionString, string commandName, IDictionary<string, object> parameters, CancellationToken cancellationToken)
        {
            return await InternalExecuteSqlCommandAsync(connectionString, commandName, parameters != null ? GenerateParameters(parameters) : null, RunExecuteNonQueryAsync, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Выполнить sql хранимую процедуру, возвращающую данные
        /// </summary>
        /// <param name="connectionString">Строка соединения с БД</param>
        /// <param name="commandName">Строка соединения с БД</param>
        /// <param name="parameters">Входные параметры, преобразуемые в параметры хранимой процедуры</param>
        /// <param name="settings">Запрашивать статистику выполнения команды</param>
        /// <returns>Набор - заполненный данными и выходные параметры</returns>
        public TResult ExecuteQuery(string connectionString, string commandName, IDictionary<string, object> parameters)
        {
            return InternalExecuteSqlCommand(connectionString, commandName, parameters != null ? GenerateParameters(parameters) : null, RunExecuteReader);
        }

        public async Task<TResult> ExecuteQueryAsync(string connectionString, string commandName, IDictionary<string, object> parameters, CancellationToken cancellationToken)
        {
            return await InternalExecuteSqlCommandAsync(connectionString, commandName, parameters != null ? GenerateParameters(parameters) : null, RunExecuteReaderAsync, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Генерация параметров хранимой процедуры
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected virtual TParameter[] GenerateParameters(IDictionary<string, object> parameters)
        {
            var result = new List<TParameter>();

            var currentParameter = default(TParameter);
            foreach (var item in parameters)
            {
                currentParameter = CreateParameter(MapType(item.Value != null ? item.Value.GetType().GetTypeInfo() : null));

                currentParameter.ParameterName = item.Key;
                currentParameter.Value = GetValue(item.Value);

                if (item.Key.ToLower().StartsWith(ParametersExtensions.OutParamPrefixName, StringComparison.OrdinalIgnoreCase))
                {
                    currentParameter.ParameterName = item.Key.Remove(0, ParametersExtensions.OutParamPrefixName.Length);
                    currentParameter.Direction = ParameterDirection.Output;
                }

                result.Add(currentParameter);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Маппинг CLR типов в SQL типы
        /// </summary>
        /// <param name="valueTypeInfo"></param>
        /// <returns></returns>
        protected virtual TDbType MapType(TypeInfo valueTypeInfo)
        {
            TDbType result = default(TDbType);

            if (valueTypeInfo != null)
            {
                if (!_typeMap.TryGetValue(!valueTypeInfo.IsEnum ? valueTypeInfo : typeof(int).GetTypeInfo(), out result))
                {
                    throw new NotSupportedException($"Тип : {valueTypeInfo} не поддерживается clr -> sql маппингом");
                }
            }

            return result;
        }
        /// <summary>
        /// Вернуть значения для объекта - внутри могут быть преобразования
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        protected virtual object GetValue(object val)
        {
            object result = DBNull.Value;

            if (val != null)
            {
                if (val is IGetObjectValue)
                {
                    result = ((IGetObjectValue)val).GetValue();
                }
                else
                {
                    result = val;
                }
            }

            return result;
        }

        protected virtual void AfterConnect(TConnection connection)
        { }

        protected virtual void TakeConnectionData(TConnection connection)
        { }

        protected abstract TConnection CreateConnection(string connectionName);

        protected abstract TCommand CreateCommand();

        protected abstract TParameter CreateParameter(TDbType dbType);

        protected abstract TResult CreateResult(ICollection<DataItem> returnedData, IDictionary<string, object> outputParameters);

        private void RunExecuteNonQuery(TCommand command, ICollection<DataItem> collection)
        {
            command.ExecuteNonQuery();
        }

        private async Task RunExecuteNonQueryAsync(TCommand command, ICollection<DataItem> collection, CancellationToken token)
        {
            await command.ExecuteNonQueryAsync(token).ConfigureAwait(false);
        }

        private void RunExecuteReader(TCommand command, ICollection<DataItem> collection)
        {
            var reader = command.ExecuteReader();

            int resultIndex = 0;
            do
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var rows = new DataItemElement[reader.FieldCount];
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var data = reader[i];
                            rows[i] = new DataItemElement(reader.GetName(i), data.GetType(), data);
                        }

                        collection.Add(new DataItem { Index = resultIndex, Items = rows });
                    }
                }
                resultIndex++;
            }
            while (reader.NextResult());
        }

        private async Task RunExecuteReaderAsync(TCommand command, ICollection<DataItem> collection, CancellationToken token)
        {
            var reader = await command.ExecuteReaderAsync(token).ConfigureAwait(false);

            int resultIndex = 0;
            do
            {
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync(token).ConfigureAwait(false))
                    {
                        var rows = new DataItemElement[reader.FieldCount];
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var data = reader[i];
                            rows[i] = new DataItemElement(reader.GetName(i), data.GetType(), data);
                        }

                        collection.Add(new DataItem { Index = resultIndex, Items = rows });
                    }
                }
                resultIndex++;
            }
            while (await reader.NextResultAsync(token).ConfigureAwait(false));
        }

        private void SetCommandType(TCommand command, string commandName)
        {
            Guard.ArgumentNotNull(command);

            if (_commandTextMap.ContainsKey(commandName))
            {
                command.CommandType = CommandType.Text;
                command.CommandText = _commandTextMap[commandName];
            }
            else
            {
                command.CommandType = CommandType.StoredProcedure;
            }
        }

        /// <summary>
        /// Выполнение sql хранимой процедуры
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="commandName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private TResult InternalExecuteSqlCommand(string connectionString, string commandName, TParameter[] parameters, Action<TCommand, ICollection<DataItem>> action)
        {
            var dataResult = new List<DataItem>();
            var outputParametersResult = new Dictionary<string, object>();

            using (var connection = CreateConnection(connectionString))
            {
                var command = CreateCommand();

                command.CommandText = commandName;
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = connection.ConnectionTimeout;

                SetCommandType(command, commandName);

                using (command)
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    command.Parameters.Add(
                        CreateParameter(default(TDbType))
                            .FluentIt(
                                p =>
                                {
                                    p.ParameterName = ParametersExtensions.ReturnValueParamName;
                                    p.Value = null;
                                    p.Direction = ParameterDirection.ReturnValue;
                                }));

                    connection.Open();

                    AfterConnect(connection);

                    action(command, dataResult);

                    foreach (DbParameter item in command.Parameters)
                    {
                        if ((item.Direction == ParameterDirection.Output) || (item.Direction == ParameterDirection.ReturnValue))
                        {
                            outputParametersResult.Add(
                                item.Direction == ParameterDirection.Output ?
                                    string.Concat(ParametersExtensions.OutParamPrefixName, item.ParameterName) :
                                    item.ParameterName,
                                ((item.Value != null) && (item.Value.GetType() != typeof(DBNull))) ?
                                    item.Value :
                                    null);
                        }
                    }
                }

                TakeConnectionData(connection);
            }

            return CreateResult(dataResult, outputParametersResult);
        }

        private async Task<TResult> InternalExecuteSqlCommandAsync(string connectionString, string commandName, TParameter[] parameters, Func<TCommand, ICollection<DataItem>, CancellationToken, Task> action, CancellationToken token)
        {
            var dataResult = new List<DataItem>();
            var outputParametersResult = new Dictionary<string, object>();

            using (var connection = CreateConnection(connectionString))
            {
                var command = CreateCommand();

                command.CommandText = commandName;
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = connection.ConnectionTimeout;

                SetCommandType(command, commandName);

                using (command)
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    command.Parameters.Add(
                        CreateParameter(default(TDbType))
                            .FluentIt(
                                p =>
                                {
                                    p.ParameterName = ParametersExtensions.ReturnValueParamName;
                                    p.Value = null;
                                    p.Direction = ParameterDirection.ReturnValue;
                                }));

                    await connection.OpenAsync(token);

                    AfterConnect(connection);

                    await action(command, dataResult, token);

                    foreach (DbParameter item in command.Parameters)
                    {
                        if ((item.Direction == ParameterDirection.Output) || (item.Direction == ParameterDirection.ReturnValue))
                        {
                            outputParametersResult.Add(
                                item.Direction == ParameterDirection.Output ?
                                    string.Concat(ParametersExtensions.OutParamPrefixName, item.ParameterName) :
                                    item.ParameterName,
                                ((item.Value != null) && (item.Value.GetType() != typeof(DBNull))) ?
                                    item.Value :
                                    null);
                        }
                    }
                }

                TakeConnectionData(connection);
            }

            return CreateResult(dataResult, outputParametersResult);
        }
    }
}
