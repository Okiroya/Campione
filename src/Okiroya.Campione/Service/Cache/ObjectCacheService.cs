using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Okiroya.Campione.SystemUtility;

namespace Okiroya.Campione.Service.Cache
{
    /// <summary>
    /// Реализация кэш-сервиса, использующая в качестве хранилища ObjectCache
    /// </summary>
    public class ObjectCacheService : BaseCacheService //TODO: remove code clones
    {
        private IDistributedCache _currentCache = null;

        private string _cacheRegion;

        public ObjectCacheService(IDistributedCache cache, string region = null)
        {
            Guard.ArgumentNotNull(cache);

            _currentCache = cache;

            _cacheRegion = region;
        }

        public override void Add(string commandName, string cacheKey, byte[] data)
        {
            Guard.ArgumentNotEmpty(commandName);
            Guard.ArgumentNotEmpty(cacheKey);
            Guard.ArgumentNotNull(data);

            var cachePolicy = CachePolicyRegister.GetPolicy(commandName);

            var options = new DistributedCacheEntryOptions().FluentIt(
                (option) =>
                {
                    option.AbsoluteExpiration = DateTimeOffset.Now.Add(cachePolicy.AbsolutExpiration);
                    option.SlidingExpiration = cachePolicy.SlidingExpiration;
                });

            _currentCache.Set(cacheKey, data, options);
        }

        public override async Task AddAsync(string commandName, string cacheKey, byte[] data)
        {
            Guard.ArgumentNotEmpty(commandName);
            Guard.ArgumentNotEmpty(cacheKey);
            Guard.ArgumentNotNull(data);

            var cachePolicy = CachePolicyRegister.GetPolicy(commandName);

            var options = new DistributedCacheEntryOptions().FluentIt(
                (option) =>
                {
                    option.AbsoluteExpiration = DateTimeOffset.Now.Add(cachePolicy.AbsolutExpiration);
                    option.SlidingExpiration = cachePolicy.SlidingExpiration;
                });

            await _currentCache.SetAsync(cacheKey, data, options).ConfigureAwait(false);
        }

        public override byte[] GetData(string cacheKey)
        {
            Guard.ArgumentNotEmpty(cacheKey);

            var result = _currentCache.Get(cacheKey);
            if (result != null)
            {
                _currentCache.Refresh(cacheKey);
            }

            return result;
        }

        public override async Task<byte[]> GetDataAsync(string cacheKey)
        {
            Guard.ArgumentNotEmpty(cacheKey);

            var result = await _currentCache.GetAsync(cacheKey).ConfigureAwait(false);
            if (result != null)
            {
                await _currentCache.RefreshAsync(cacheKey).ConfigureAwait(false);
            }

            return result;
        }

        public override void SetData(string commandName, string cacheKey, byte[] data)
        {
            Guard.ArgumentNotEmpty(cacheKey);

            var cachePolicy = CachePolicyRegister.GetPolicy(commandName);

            var options = new DistributedCacheEntryOptions().FluentIt(
                (option) =>
                {
                    option.AbsoluteExpiration = DateTimeOffset.Now.Add(cachePolicy.AbsolutExpiration);
                    option.SlidingExpiration = cachePolicy.SlidingExpiration;
                });

            _currentCache.Set(cacheKey, data, options);
        }

        public override async Task SetDataAsync(string commandName, string cacheKey, byte[] data)
        {
            Guard.ArgumentNotEmpty(cacheKey);

            var cachePolicy = CachePolicyRegister.GetPolicy(commandName);

            var options = new DistributedCacheEntryOptions().FluentIt(
                (option) =>
                {
                    option.AbsoluteExpiration = DateTimeOffset.Now.Add(cachePolicy.AbsolutExpiration);
                    option.SlidingExpiration = cachePolicy.SlidingExpiration;
                });

            await _currentCache.SetAsync(cacheKey, data, options).ConfigureAwait(false);
        }

        public override void Remove(string cacheKey)
        {
            Guard.ArgumentNotEmpty(cacheKey);

            _currentCache.Remove(cacheKey);
        }

        public override async Task RemoveAsync(string cacheKey)
        {
            Guard.ArgumentNotEmpty(cacheKey);

            await _currentCache.RemoveAsync(cacheKey).ConfigureAwait(false);
        }

        public override string GenerateCacheKey(string commandName, IDictionary<string, object> parameters)
        {
            return !string.IsNullOrEmpty(_cacheRegion) ?
                string.Concat(_cacheRegion, "::", base.GenerateCacheKey(commandName, parameters)) :
                base.GenerateCacheKey(commandName, parameters);
        }
    }
}
