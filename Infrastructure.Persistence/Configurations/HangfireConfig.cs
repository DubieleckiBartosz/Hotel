using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Configurations
{
    public static class HangfireConfig
    {
        public static IServiceCollection GetHangfireConfig(this IServiceCollection services)
        {
            //services.AddHangfire(x => x.UseSqlServerStorage("DefaultConnection"));
            //services.AddHangfireServer();

            return services;
        }
        //public static IApplicationBuilder GetDashBoardHangfire(this IApplicationBuilder app)=>
        //    app.UseHangfireDashboard("/mydashboard");
    }
}
