using Application.Cache;
using Application.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.WebApi.Cache
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CacheAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveSeconds;
        private readonly int _unusedExpireTime;
        public CacheAttribute()
        {

        }
        public CacheAttribute(int timeToLiveSeconds)
        {
            _timeToLiveSeconds = timeToLiveSeconds;
        }
        public CacheAttribute(int timeToLiveSeconds,int unusedExpireTime)
            :this(timeToLiveSeconds)
        {
            _unusedExpireTime = unusedExpireTime;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            var cacheSettings = context.HttpContext.RequestServices.GetRequiredService<RedisCacheSettings>();

            if (!cacheSettings.Enabled)
            {
                await next();
                return;
            }

            var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheResponse>();

            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
            var cachedResponse = await cacheService.GetCachedResponseAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedResponse))
            {
                var contentResult = new ContentResult
                {
                    Content = cachedResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = contentResult;
                return;
            }

            var executedContext = await next();

            if (executedContext.Result is OkObjectResult okObjectResult)
            {

                if (_timeToLiveSeconds==default && _unusedExpireTime==default)
                {
                    await cacheService.CacheResponseAsync(cacheKey, okObjectResult.Value);
                }
                else if (_timeToLiveSeconds != default && _unusedExpireTime == default)
                {
                    await cacheService.CacheResponseAsync(cacheKey, okObjectResult.Value,
                        TimeSpan.FromSeconds(_timeToLiveSeconds));
                }
                else if (_timeToLiveSeconds != default && _unusedExpireTime != default
                                                  && _timeToLiveSeconds > _unusedExpireTime)
                {
                    await cacheService.CacheResponseAsync(cacheKey, okObjectResult.Value,
                        TimeSpan.FromSeconds(_timeToLiveSeconds), TimeSpan.FromSeconds(_unusedExpireTime));
                }
                else
                {
                    throw new Exception("Incorrect time data has been entered ");
                }
            }
        }

        private static string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();

            keyBuilder.Append($"{request.Path}");

            foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                keyBuilder.Append($"|{key}-{value}");
            }

            return keyBuilder.ToString();
        }
    }

}
