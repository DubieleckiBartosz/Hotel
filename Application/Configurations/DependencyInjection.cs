using Application.Contracts;
using Application.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Configurations
{
    public static class DependencyInjection
    {
        public static IServiceCollection ApplicationDependency(this IServiceCollection services)
        {
            services.AddScoped<IUserContextService,UserContextService>();
            return services;
        }
    }
}
