using Application.Contracts;
using Domain.Settings;
using Infrastructure.File;
using Infrastructure.Mail;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection GetInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<ICsvExporter, CsvExporter>();
            services.AddTransient<IEmailService, EmailService>();

            return services;
        }
    }
}
