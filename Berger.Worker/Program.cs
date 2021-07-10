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
using Berger.Worker.Model;
using System.IO;
using Berger.Odata.Model;
using Berger.Odata.Services;
using Serilog;
using Serilog.Events;

namespace Berger.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var _configuration = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
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
                    string connectionString = hostContext.Configuration.GetConnectionString(nameof(ApplicationDbContext));
                    services.Configure<WorkerSettingsModel>(options => hostContext.Configuration.GetSection("WorkerSettings").Bind(options));
                    services.Configure<WorkerConfig>(options => hostContext.Configuration.GetSection("WorkerConfig").Bind(options));
                    services.Configure<ODataSettingsModel>(options => hostContext.Configuration.GetSection("ODataSettings").Bind(options));
                    services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
                    services.AddScoped<DbContext, ApplicationDbContext>();
                    services.AddScoped<ICustomerService, CustomerService>();
                    services.AddScoped<IBrandService, BrandService>();
                    services.AddScoped<IBrandFamilyService, BrandFamilyService>();
                    services.AddScoped<IHttpClientService, HttpClientService>();
                    services.AddScoped(typeof(IDataEqualityComparer<>), typeof(DataEqualityComparer<>));
                    services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
                    services.AddScoped<IUnitOfWork, ApplicationDbContext>();
                    services.AddScoped<IWorkerSyncService, WorkerSyncService>();
                    services.AddScoped<IODataService, ODataService>();
                    services.AddScoped<ISyncService, SyncService>();
                    services.AddHostedService<Worker>();
                    services.AddHostedService<DailySalesNTargetDataWorker>();
                });
    }
}
