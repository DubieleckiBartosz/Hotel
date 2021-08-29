

using Infrastructure.Identity.DbContext;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Identity.Configurations
{
    public static class IdentityDb
    {
        public static void GetIdentityDb(this IServiceCollection services,IConfiguration configuration)
        {

            services.AddDbContext<IdentityContext>(options =>
            
            options.UseSqlServer(configuration.GetConnectionString("IdentityConnection")));
        }
    }
}
