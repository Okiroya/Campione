using Microsoft.Extensions.Caching.Distributed;
using Okiroya.Campione.DataAccess;
using Okiroya.Campione.SystemUtility;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Okiroya.Campione.Service.Cache
{
    public sealed class EntityServiceObjectCache : IDistributedCache, IDisposable
    {
        private static readonly Task _completedTask = Task.FromResult<object>(null);
        private static readonly TimeSpan _oneDay = new TimeSpan(1, 0, 0, 0);
        
        private bool _disposed;

        public byte[] Get(string key)
        {
            Guard.ArgumentNotEmpty(key);

            if (_disposed)
            {
                return null;
            }
            else
            {
                var parameters = new Dictionary<string, object>
                {
                    { "CacheKey", PrepareKey(key) },
                    { "UtcTime", DateTime.UtcNow.Ticks }
                };

                return EntityServiceFacade<CacheEntryEntity, long>.GetItem(EntityServiceObjectCacheDataConfig.GetItem, parameters).Entry;
            }
        }

        public async Task<byte[]> GetAsync(string key)
        {
            Guard.ArgumentNotEmpty(key);

            if (_disposed)
            {
                return null;
            }
            else
            {
                var parameters = new Dictionary<string, object>
                {
                    { "CacheKey", PrepareKey(key) },
                    { "UtcTime", DateTime.UtcNow.Ticks }
                };

                var cacheEntry = await EntityServiceFacade<CacheEntryEntity, long>.GetItemAsync(EntityServiceObjectCacheDataConfig.GetItem, parameters, CancellationToken.None).ConfigureAwait(false);

                return cacheEntry?.Entry;
            }
        }

        public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
        {
            Guard.ArgumentNotEmpty(key);
            Guard.ArgumentNotNull(value);

            DateTimeOffset? absExp = CachePolicy.InfiniteAbsoluteExpiration;
            TimeSpan? slidingExp = CachePolicy.NoSlidingExpiration;

            if (options != null)
            {
                ValidatePolicy(options);

                absExp = options.AbsoluteExpiration;
                slidingExp = options.SlidingExpiration;
            }

            if (!_disposed)
            {
                var entryType = value.GetType().AssemblyQualifiedName;

                var parameters = new Dictionary<string, object>
                {
                    { "CacheKey", PrepareKey(key) },
                    { "UtcTime", DateTime.UtcNow.Ticks },
                    { "UtcAbsoluteExpiration", absExp?.UtcTicks },
                    { "SlidingExpiration", slidingExp?.Ticks },
                    { "EntryType", entryType },
                    { "Entry", CacheEntrySerializerFacade.Serialize(entryType, value) },
                };

                EntityServiceFacade<CacheEntryEntity, long>.ExecuteCommand(EntityServiceObjectCacheDataConfig.SetItem, parameters);
            }
        }

        public async Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options)
        {
            Guard.ArgumentNotEmpty(key);
            Guard.ArgumentNotNull(value);

            DateTimeOffset? absExp = CachePolicy.InfiniteAbsoluteExpiration;
            TimeSpan? slidingExp = CachePolicy.NoSlidingExpiration;

            if (options != null)
            {
                ValidatePolicy(options);

                absExp = options.AbsoluteExpiration;
                slidingExp = options.SlidingExpiration;
            }

            if (!_disposed)
            {
                var entryType = value.GetType().AssemblyQualifiedName;

                var parameters = new Dictionary<string, object>
                {
                    { "CacheKey", PrepareKey(key) },
                    { "UtcTime", DateTime.UtcNow.Ticks },
                    { "UtcAbsoluteExpiration", absExp?.UtcTicks },
                    { "SlidingExpiration", slidingExp?.Ticks },
                    { "EntryType", entryType },
                    { "Entry", CacheEntrySerializerFacade.Serialize(entryType, value) },
                };

                await EntityServiceFacade<CacheEntryEntity, long>.ExecuteCommandAsync(EntityServiceObjectCacheDataConfig.SetItem, parameters, CancellationToken.None).ConfigureAwait(false);
            }
        }

        public void Refresh(string key)
        { }

        public Task RefreshAsync(string key)
        {
            return _completedTask;
        }

        public void Remove(string key)
        {
            Guard.ArgumentNotEmpty(key);

            if (!_disposed)
            {
                var parameters = new Dictionary<string, object>
                {
                    { "CacheKey", PrepareKey(key) },
                    { "UtcTime", DateTime.UtcNow.Ticks },
                    {  ParametersExtensions.MakeOutParamName("Exists"), ParametersExtensions.DefaultParamValue<bool>() }
                };

                EntityServiceFacade<CacheEntryEntity, long>.ExecuteCommand(EntityServiceObjectCacheDataConfig.RemoveItem, parameters);
            }
        }

        public async Task RemoveAsync(string key)
        {
            Guard.ArgumentNotEmpty(key);

            if (!_disposed)
            {
                var parameters = new Dictionary<string, object>
                {
                    { "CacheKey", PrepareKey(key) },
                    { "UtcTime", DateTime.UtcNow.Ticks },
                    {  ParametersExtensions.MakeOutParamName("Exists"), ParametersExtensions.DefaultParamValue<bool>() }
                };

                await EntityServiceFacade<CacheEntryEntity, long>.ExecuteCommandAsync(EntityServiceObjectCacheDataConfig.RemoveItem, parameters, CancellationToken.None).ConfigureAwait(false);
            }
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        private void ValidatePolicy(DistributedCacheEntryOptions options)
        {
            if ((options.AbsoluteExpiration != CachePolicy.InfiniteAbsoluteExpiration) && (options.SlidingExpiration != CachePolicy.NoSlidingExpiration))
            {
                throw new ArgumentException("Неверно заданы свойства: можно задать либо AbsoluteExpiration, либо SlidingExpiration, но не оба свойства сразу", "policy");
            }

            if ((options.SlidingExpiration < CachePolicy.NoSlidingExpiration) || (_oneDay < options.SlidingExpiration))
            {
                throw new ArgumentOutOfRangeException("policy", string.Format("Значение свойства SlidingExpiration должно быть >= {0} and =< {1}", CachePolicy.NoSlidingExpiration, _oneDay));
            }
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _disposed = true;
            }
        }

        private static long PrepareKey(string cacheKey)
        {
            return OneAtaTimeHash.GetHash(cacheKey);
        }
    }
}
