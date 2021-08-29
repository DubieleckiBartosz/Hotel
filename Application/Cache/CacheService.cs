using Application.Contracts;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Cache
{
    public class CacheService : ICacheResponse
    {
        private readonly IDistributedCache _cache;
        public CacheService(IDistributedCache cache)
        { 
            _cache = cache;
        }
        public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan? absoluteExpireTime = null, TimeSpan? unusedExpireTime = null)
        {
            var options = new DistributedCacheEntryOptions();
            options.AbsoluteExpirationRelativeToNow = absoluteExpireTime ??
                TimeSpan.FromSeconds(60 * 20);
            options.SlidingExpiration = unusedExpireTime ?? TimeSpan.FromSeconds(60);

            var json = JsonConvert.SerializeObject(response);
            await _cache.SetStringAsync(cacheKey, json, options);
        }

        public async Task<string> GetCachedResponseAsync(string cacheKey)
        {
            var data = await _cache.GetStringAsync(cacheKey);
            if (data is null)
            {
                return default;
            }
            return data;
        }
    }
}
