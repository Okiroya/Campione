using System;
using System.Collections.Generic;

namespace Okiroya.Campione.Service.Cache
{
    /// <summary>
    /// Заглушка для кэш-сервиса. Ничего не делает
    /// </summary>
    public class NoCacheService : BaseCacheService
    {
        /// <summary>
        /// Заглушка
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="cacheKey"></param>
        /// <param name="data"></param>
        public override void Add(string commandName, string cacheKey, byte[] data)
        { }

        /// <summary>
        /// Заглушка
        /// </summary>
        /// <param name="cacheKey"></param>
        public override void Remove(string cacheKey)
        { }

        /// <summary>
        /// Всегда возвращает null
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public override byte[] GetData(string cacheKey)
        {
            return null;
        }

        /// <summary>
        /// Заглушка
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="cacheKey"></param>
        /// <param name="data"></param>
        public override void SetData(string commandName, string cacheKey, byte[] data)
        { }

        /// <summary>
        /// Всегда возвращает null - это особенность используется в CacheFacade
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public override string GenerateCacheKey(string commandName, IDictionary<string, object> parameters)
        {
            return null;
        }
    }
}
