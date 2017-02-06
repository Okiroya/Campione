using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using Okiroya.Campione.SystemUtility;
using Okiroya.Campione.SystemUtility.DI;

namespace Okiroya.Campione.Service.Cache
{
    /// <summary>
    /// Фасад для доступа к кэш-сервису
    /// </summary>
    public static class CacheFacade //TODO add cache dependency
    {
        private static ConcurrentDictionary<Type, string> _cacheSerializerTypeMap = new ConcurrentDictionary<Type, string>();

        private static ConcurrentDictionary<string, IList<string>> _cacheRegister = new ConcurrentDictionary<string, IList<string>>();

        /// <summary>
        /// Регистрация кэш-сервиса по-умолчанию
        /// </summary>
        static CacheFacade()
        {
            RegisterDependencyContainer<ICacheService>.SetDefault(new NoCacheService());
        }

        /// <summary>
        /// Вернуть значение из кэша, либо добавить в кэш. Кэшируются только результаты выполнения команд на чтение данных
        /// </summary>
        /// <param name="commandName">Наименование команды</param>
        /// <param name="parameters">Входные параметры</param>
        /// <param name="dataLoader">Делегат выборки данных из источника данных</param>
        /// <returns>Данные, возвращаемые сервисом доступа к данным</returns>
        public static T AddOrGetExisting<T>(string commandName, IDictionary<string, object> parameters, Func<T> dataLoader)
        {
            Guard.ArgumentNotEmpty(commandName);
            Guard.ArgumentNotNull(dataLoader);

            T result = default(T);

            var cacheService = RegisterDependencyContainer<ICacheService>.Resolve(commandName);

            string cacheKey = cacheService.GenerateCacheKey(commandName, parameters);
            if (!string.IsNullOrWhiteSpace(cacheKey))
            {
                UpdateCacheRegister(commandName, cacheKey);

                result = cacheService.AddOrGetExisting(
                    commandName: commandName,
                    cacheKey: cacheKey,
                    dataLoader: dataLoader,
                    serializator: (p) => CacheEntrySerializerFacade.Serialize(GetTypeMap<T>(), p));
            }
            else
            {
                result = dataLoader();
            }

            return result;
        }

        /// <summary>
        /// Взять значение из кэша для команды
        /// </summary>
        /// <typeparam name="T">Тип возвращаемого значения</typeparam>
        /// <param name="commandName">Наименование команды</param>
        /// <param name="parameters">Параметры команды</param>
        /// <returns></returns>
        public static T Get<T>(string commandName, IDictionary<string, object> parameters)
        {
            Guard.ArgumentNotEmpty(commandName);

            T result = default(T);

            var cacheService = RegisterDependencyContainer<ICacheService>.Resolve(commandName);

            string cacheKey = cacheService.GenerateCacheKey(commandName, parameters);
            if (!string.IsNullOrWhiteSpace(cacheKey))
            {
                result = (T)CacheEntrySerializerFacade.Deserialize(GetTypeMap<T>(), cacheService.GetData(cacheKey));
            }

            return result;
        }

        /// <summary>
        /// Положить в кэш значение для команды
        /// </summary>
        /// <remarks>
        /// Метод рекомендуется использовать только для обновления значения в кэше, т.к. в противном случае теряется инвалидация кэша по зависимостям
        /// </remarks>
        /// <typeparam name="T">Тип передаваемого в кэш значения</typeparam>
        /// <param name="commandName">Наименование команды</param>
        /// <param name="parameters">Параметры команды</param>
        /// <param name="data">Значение</param>
        public static void Set<T>(string commandName, IDictionary<string, object> parameters, T data)
        {
            Guard.ArgumentNotEmpty(commandName);

            var cacheService = RegisterDependencyContainer<ICacheService>.Resolve(commandName);

            string cacheKey = cacheService.GenerateCacheKey(commandName, parameters);
            if (!string.IsNullOrWhiteSpace(cacheKey))
            {
                UpdateCacheRegister(commandName, cacheKey);

                cacheService.SetData(commandName, cacheKey, CacheEntrySerializerFacade.Serialize(GetTypeMap<T>(), data));
            }
        }

        /// <summary>
        /// Удалить зависимые значение из кэша для команды
        /// </summary>
        /// <param name="commandName">Наименование команды</param>
        public static void Remove(string commandName)
        {
            Guard.ArgumentNotEmpty(commandName);

            IList<string> dependencies;
            if (_cacheRegister.TryGetValue(commandName, out dependencies))
            {
                if ((dependencies != null) && dependencies.Any())
                {
                    var cacheService = RegisterDependencyContainer<ICacheService>.Resolve(commandName);

                    foreach (var item in dependencies)
                    {
                        cacheService.Remove(item);
                    }
                }
            }
        }

        /// <summary>
        /// Удалить зависимые значение из кэша для команд
        /// </summary>
        /// <param name="commands">Список команд</param>
        public static void Remove(IEnumerable<string> commands)
        {
            Guard.ArgumentNotNull(commands);

            foreach (var item in commands)
            {
                Remove(item);
            }
        }

        private static void UpdateCacheRegister(string commandName, string cacheKey)
        {
            _cacheRegister.AddOrUpdate(
                commandName,
                new List<string>(new[] { cacheKey }),
                (key, oldValue) =>
                {
                    return oldValue.Zip(
                        new[] { cacheKey },
                        (first, second) =>
                        {
                            return first.Equals(second, StringComparison.OrdinalIgnoreCase) ?
                                first :
                                second;
                        })
                        .ToList();
                });
        }

        private static string GetTypeMap<T>()
        {
            var type = typeof(T);

            return _cacheSerializerTypeMap.GetOrAdd(type, (p) => p.AssemblyQualifiedName);
        }
    }
}
