using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using Okiroya.Campione.SystemUtility;
using System.Threading.Tasks;

namespace Okiroya.Campione.Service.Cache
{
    /// <summary>
    /// Базовая реализация кэш-сервиса 
    /// </summary>
    public abstract class BaseCacheService : ICacheService, IDisposable
    {
        private static readonly Task _completedTask = Task.FromResult<object>(null);

        private HybridLock _lock = new HybridLock();
        private bool _disposed;
        
        /// <summary>
        /// Добавить значение в кэш
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="cacheKey">Ключ кэша</param>
        /// <param name="data">Значение на добавление</param>
        public abstract void Add(string commandName, string cacheKey, byte[] data);

        public virtual Task AddAsync(string commandName, string cacheKey, byte[] data)
        {
            Guard.ArgumentNotEmpty(commandName);
            Guard.ArgumentNotEmpty(cacheKey);
            Guard.ArgumentNotNull(data);

            Add(commandName, cacheKey, data);

            return _completedTask;
        } 

        /// <summary>
        /// Вернуть значение из кэша
        /// </summary>
        /// <param name="cacheKey">Ключ кэша</param>
        /// <returns></returns>
        public abstract byte[] GetData(string cacheKey);

        public virtual Task<byte[]> GetDataAsync(string cacheKey)
        {
            Guard.ArgumentNotEmpty(cacheKey);

            return Task.FromResult(GetData(cacheKey));
        }

        /// <summary>
        /// Положить значение в кэш
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="cacheKey">Ключ кэша</param>
        /// <param name="data">Значение на замену</param>
        public abstract void SetData(string commandName, string cacheKey, byte[] data);

        public virtual Task SetDataAsync(string commandName, string cacheKey, byte[] data)
        {
            Guard.ArgumentNotEmpty(commandName);
            Guard.ArgumentNotEmpty(cacheKey);
            Guard.ArgumentNotNull(data);

            SetData(commandName, cacheKey, data);

            return _completedTask;
        }

        /// <summary>
        /// Удалить значение из кэша
        /// </summary>
        /// <param name="cacheKey">Ключ кэша</param>
        public abstract void Remove(string cacheKey);

        public virtual Task RemoveAsync(string cacheKey)
        {
            Guard.ArgumentNotEmpty(cacheKey);

            Remove(cacheKey);

            return _completedTask;
        }

        /// <summary>
        /// Добавить в значение в кэш если нет записи с заданным ключом, либо вернуть данные из кэша
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandName"></param>
        /// <param name="cacheKey">Ключ кэша</param>
        /// <param name="dataLoader">Делегат выборки данных из источника данных</param>
        /// <param name="serializator">Функция сериализации</param>
        /// <returns>Данные, возвращаемые сервисом доступа к данным</returns>
        public virtual T AddOrGetExisting<T>(string commandName, string cacheKey, Func<T> dataLoader, Func<T, byte[]> serializator)
        {
            Guard.ArgumentNotEmpty(commandName);
            Guard.ArgumentNotEmpty(cacheKey);
            Guard.ArgumentNotNull(dataLoader);
            Guard.ArgumentNotNull(dataLoader);

            try
            {
                _lock.Enter();

                T result = default(T);

                var data = GetData(cacheKey);

                if (data == null)
                {
                    result = dataLoader();

                    Add(commandName, cacheKey, serializator(result));
                }

                return result;
            }
            finally
            {
                _lock.Leave();
            }
        }

        public async Task<T> AddOrGetExistingAsync<T>(string commandName, string cacheKey, Func<T> dataLoader, Func<T, byte[]> serializator)
        {
            Guard.ArgumentNotEmpty(cacheKey);
            Guard.ArgumentNotNull(dataLoader);

            try
            {
                _lock.Enter();

                T result = default(T);

                var data = await GetDataAsync(cacheKey).ConfigureAwait(false);

                if (data == null)
                {
                    result = dataLoader();

                    await AddAsync(commandName, cacheKey, serializator(result)).ConfigureAwait(false);
                }

                return result;
            }
            finally
            {
                _lock.Leave();
            }
        }

        /// <summary>
        /// Сгенерировать ключ кэша
        /// </summary>
        /// <param name="commandName">Наименование команды</param>
        /// <param name="parameters">Параметры команды</param>
        /// <returns>Ключ</returns>
        public virtual string GenerateCacheKey(string commandName, IDictionary<string, object> parameters)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}+{1}", commandName,
                (parameters != null) && parameters.Any() ?
                    parameters.Select(
                        p => p.Key.EndsWith("Id", StringComparison.OrdinalIgnoreCase) ?
                            string.Format(CultureInfo.InvariantCulture, "{0}={1}", p.Key, p.Value.ToString()) :
                            p.Key)
                        .Aggregate((current, next) => string.Format(CultureInfo.InvariantCulture, "{0}_{1}", current, next)) :
                    string.Empty);
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                if (_lock != null)
                {
                    _lock.Dispose();
                }
            }

            _disposed = true;
        }
    }
}
