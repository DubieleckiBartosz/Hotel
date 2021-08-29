using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface ICacheResponse
    {
        Task CacheResponseAsync(string cacheKey, object response, TimeSpan? absoluteExpireTime = null,
                   TimeSpan? unusedExpireTime = null);
        Task<string> GetCachedResponseAsync(string cacheKey);
    }
}
