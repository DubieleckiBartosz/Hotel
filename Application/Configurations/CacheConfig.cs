using Application.Cache;
using Application.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Configurations
{
    public static class CacheConfig
    {
        public static void GetCache(this IServiceCollection services)
        {

            services.AddStackExchangeRedisCache(options =>
            {
                options.InstanceName = "Hotel_Application";
            });
            services.AddSingleton<ICacheResponse, CacheService>();
        }
    }
}
