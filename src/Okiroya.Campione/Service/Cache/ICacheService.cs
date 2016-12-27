using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Okiroya.Campione.Service.Cache
{
    /// <summary>
    /// Контракт для кэш-сервиса
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// Добавить значение в кэш
        /// </summary>
        /// <param name="commandName">Наименование команды сервиса, для которой данные добавляются в кэш</param>
        /// <param name="cacheKey">Ключ кэша</param>
        /// <param name="data">Значение на добавление</param>
        void Add(string commandName, string cacheKey, byte[] data);

        Task AddAsync(string commandName, string cacheKey, byte[] data);

        /// <summary>
        /// Добавить в значение в кэш если нет записи с заданным ключом, либо вернуть данные из кэша
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandName">Наименование команды сервиса, для которой данные добавляются в кэш</param>
        /// <param name="cacheKey">Ключ кэша</param>
        /// <param name="dataLoader">Функция выборки данных из источника данных</param>
        /// <param name="serializator">Функция сериализации</param>
        /// <returns>Данные, возвращаемые сервисом доступа к данным</returns>
        T AddOrGetExisting<T>(string commandName, string cacheKey, Func<T> dataLoader, Func<T, byte[]> serializator);

        Task<T> AddOrGetExistingAsync<T>(string commandName, string cacheKey, Func<T> dataLoader, Func<T, byte[]> serializator);

        /// <summary>
        /// Вернуть значение из кэша
        /// </summary>
        /// <param name="cacheKey">Ключ кэша</param>
        /// <returns></returns>
        byte[] GetData(string cacheKey);

        Task<byte[]> GetDataAsync(string cacheKey);

        /// <summary>
        /// Положить значение в кэш
        /// </summary>
        /// <param name="commandName">Наименование команды сервиса, для которой данные устанавливаются в кэше</param>
        /// <param name="cacheKey">Ключ кэша</param>
        /// <param name="data">Значение на замену</param>
        void SetData(string commandName, string cacheKey, byte[] data);

        Task SetDataAsync(string commandName, string cacheKey, byte[] data);

        /// <summary>
        /// Удалить значение из кэша
        /// </summary>
        /// <param name="cacheKey">Ключ кэша</param>
        void Remove(string cacheKey);

        Task RemoveAsync(string cacheKey);

        /// <summary>
        /// Сгенерировать ключ кэша
        /// </summary>
        /// <param name="commandName">Наименование команды</param>
        /// <param name="parameters">Параметры команды</param>
        /// <returns>Ключ</returns>
        string GenerateCacheKey(string commandName, IDictionary<string, object> parameters);
    }
}
