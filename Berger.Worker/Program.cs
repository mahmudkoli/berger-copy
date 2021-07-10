using System;
using Berger.Common.HttpClient;
using Berger.Data.Common;
using Berger.Data.MsfaEntity;
using Berger.Worker.Common;
using Berger.Worker.Services;
using BergerMsfaApi.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Berger.Worker.Model;
using System.IO;
using Serilog;
using Serilog.Events;

namespace Berger.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var _configuration = new ConfigurationBuilder()
                                //.SetBasePath(Directory.GetCurrentDirectory())
                                //.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                                .SetBasePath(AppContext.BaseDirectory)
                                .AddJsonFile("appsettings.json", false, true)
                                .AddEnvironmentVariables()
                                .Build();

            var workerSettings = _configuration.GetSection("WorkerSettings").Get<WorkerSettingsModel>();

            Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Debug()
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                        .Enrich.FromLogContext()
                        .WriteTo.Console()
                        .WriteTo.File(workerSettings.LogUrl, rollingInterval: RollingInterval.Day)
                        .CreateLogger();

            try
            {
                Log.Information("Application Starting up");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .UseSerilog()
                .ConfigureServices((hostContext, services) =>
                {
                    services.Configure<WorkerSettingsModel>(options => hostContext.Configuration.GetSection("WorkerSettings").Bind(options));
                    services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(hostContext.Configuration.GetConnectionString(nameof(ApplicationDbContext))));
                    services.AddScoped<DbContext, ApplicationDbContext>();
                    services.AddScoped<ICustomerService, CustomerService>();
                    services.AddScoped<IBrandService, BrandService>();
                    services.AddScoped<IBrandFamilyService, BrandFamilyService>();
                    services.AddScoped<IHttpClientService, HttpClientService>();
                    services.AddScoped(typeof(IDataEqualityComparer<>), typeof(DataEqualityComparer<>));
                    services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
                    services.AddScoped<IUnitOfWork, ApplicationDbContext>();
                    services.AddHostedService<Worker>();
                });
    }
}
