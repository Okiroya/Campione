using Okiroya.Campione.SystemUtility;
using System;
using System.Collections.Concurrent;

namespace Okiroya.Campione.Service.Cache
{
    /// <summary>
    /// Реестр <see cref="CachePolicy"/> для определенных команд
    /// </summary>
    public static class CachePolicyRegister
    {    
        private static ConcurrentDictionary<string, CachePolicy> _register = new ConcurrentDictionary<string, CachePolicy>();

        /// <summary>
        /// Добавить <see cref="CachePolicy"/> для команды
        /// </summary>
        /// <param name="commandName">Наименование команды</param>
        /// <param name="policy">Характеристики времени жизни объекта в кэше</param>
        public static void AddPolicy(string commandName, CachePolicy policy)
        {
            Guard.ArgumentNotEmpty(commandName);
            Guard.ArgumentNotNull(policy);

            ValidatePolicy(policy);

            _register.AddOrUpdate(
                commandName,
                policy,
                (key, oldValue) =>
                {
                    return policy;
                });
        }

        /// <summary>
        /// Вернуть <see cref="CachePolicy"/> для команды
        /// </summary>
        /// <param name="commandName">Наименование команды</param>
        /// <returns></returns>
        public static CachePolicy GetPolicy(string commandName)
        {
            Guard.ArgumentNotEmpty(commandName);

            CachePolicy result;

            return _register.TryGetValue(commandName, out result) ?
                result :
                CachePolicy.Default;
        }

        private static void ValidatePolicy(CachePolicy policy)
        {
            if ((policy.AbsolutExpiration != TimeSpan.MaxValue) && (policy.SlidingExpiration != CachePolicy.NoSlidingExpiration))
            {
                throw new ArgumentException("Неверно заданы свойства: можно задать либо AbsoluteExpiration, либо SlidingExpiration, но не оба свойства сразу", "policy");
            }

            if ((policy.SlidingExpiration < CachePolicy.NoSlidingExpiration) || (CachePolicy.OneDaySlidingExpiration < policy.SlidingExpiration))
            {
                throw new ArgumentOutOfRangeException("policy", string.Format("Значение свойства SlidingExpiration должно быть >= {0} and =< {1}", CachePolicy.NoSlidingExpiration, CachePolicy.OneDaySlidingExpiration));
            }
        }
    }
}
