using System;

namespace Okiroya.Campione.Service.Cache
{
    /// <summary>
    /// Характеристики времени жизни объекта в кэше
    /// </summary>
    public class CachePolicy
    {
        public static readonly DateTimeOffset InfiniteAbsoluteExpiration = DateTimeOffset.MaxValue;
        public static readonly TimeSpan NoSlidingExpiration = TimeSpan.Zero;
        public static readonly TimeSpan OneDaySlidingExpiration = new TimeSpan(1, 0, 0, 0);

        /// <summary>
        /// Характеристики времени жизни объекта в кэше по умолчанию
        /// </summary>
        public static CachePolicy Default = new CachePolicy
        {
            AbsolutExpiration = new TimeSpan(0, 20, 0),//20 минут
            SlidingExpiration = NoSlidingExpiration
        };

        /// <summary>
        /// Время жизни объекта в кэше. Объект в любом случае удалится по истечении указанного промежутка времени
        /// </summary>
        public TimeSpan AbsolutExpiration { get; set; }

        /// <summary>
        /// Время жизни объекта в кэше. Объект удалится из кэша, если он не будет запрошен в течение указанного промежутка времени
        /// </summary>
        public TimeSpan SlidingExpiration { get; set; }
    }
}
