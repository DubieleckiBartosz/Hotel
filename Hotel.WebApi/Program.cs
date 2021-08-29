using Infrastructure.Identity.Seed;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Hotel.WebApi
{
    public class Program
    {
        public async static Task<int> Main(string[] args)
        {

            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                //.AddJsonFile($"appsettings." +
                //$"{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
                //optional: true)
                .Build();

            //Log.Logger = new LoggerConfiguration()
            //    .ReadFrom.Configuration(config)
            //    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
            //    .CreateLogger();


            Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
            .WriteTo.Elasticsearch(ConfigureElasticSink(config, environment))
            .Enrich.WithProperty("Environment", environment)
            .ReadFrom.Configuration(config)
            .CreateLogger();

                try
                {
                    Log.Information("Starting host");
                var host = CreateHostBuilder(args).Build();
                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    var identityRoles = services.GetRequiredService<RoleManager<IdentityRole>>();
                    await Seeder.GetRoles(identityRoles);
                }

                host.Run();
                    return 0;
                }
                catch (Exception ex)
                {
                    Log.Fatal(ex, "Host terminated unexpectedly");
                    return 1;
                }
                finally
                {
                    Log.CloseAndFlush();
                }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
            .UseSerilog()
            .ConfigureAppConfiguration(configuration =>
                {
                    configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    configuration.AddJsonFile(
                        $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
                        optional: true);
                });

        private static ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot configuration,string environment)
        {
            return new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"]))
            {
                AutoRegisterTemplate = true,
                IndexFormat = $"{configuration["ApplicationName"]}-{environment?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
            };
        }
    }
}
