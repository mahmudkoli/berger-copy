using System;
using Berger.Common.HttpClient;
using Berger.Data.Common;
using Berger.Data.MsfaEntity;
using Berger.Worker.Common;
using Berger.Worker.Services;
using Berger.Worker.Repositories;
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
using Berger.Worker.Services.AlertNotification;
using Serilog.Sinks.MSSqlServer;
using Berger.Common.Constants;

namespace Berger.Worker
{
    public class Program
    {
        //public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
        //   .SetBasePath(Directory.GetCurrentDirectory())
        //   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        //   .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
        //   .Build();

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


            var connectionStringName = "ApplicationDbContext";
            var connectionString = _configuration.GetConnectionString(connectionStringName);

            Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Debug()
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                        .Enrich.FromLogContext()
                        .WriteTo.Console()
                        //.WriteTo.File(workerSettings.LogUrl, rollingInterval: RollingInterval.Day)
                        .WriteTo.MSSqlServer(connectionString, sinkOptions: new MSSqlServerSinkOptions { TableName = ConstantsApplication.SerilogMSSqlServerTableName })
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
                    string appConnectionString = hostContext.Configuration.GetConnectionString(nameof(ApplicationDbContext));
                    string sapConnectionString = hostContext.Configuration.GetConnectionString(nameof(SAPDbContext));
                    services.Configure<WorkerSettingsModel>(options => hostContext.Configuration.GetSection("WorkerSettings").Bind(options));
                    services.Configure<WorkerConfig>(options => hostContext.Configuration.GetSection("WorkerConfig").Bind(options));
                    services.Configure<ODataSettingsModel>(options => hostContext.Configuration.GetSection("ODataSettings").Bind(options));
                    services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(appConnectionString, opt => opt.CommandTimeout(3600)));
                    services.AddDbContext<SAPDbContext>(options => options.UseSqlServer(sapConnectionString, op => op.CommandTimeout(3600)));
                    //services.AddScoped<DbContext, ApplicationDbContext>();
                    services.AddScoped<ApplicationDbContext, ApplicationDbContext>();
                    services.AddScoped<SAPDbContext, SAPDbContext>();
                    services.AddScoped<ICustomerService, CustomerService>();
                    services.AddScoped<IBrandService, BrandService>();
                    services.AddScoped<IBrandFamilyService, BrandFamilyService>();
                    services.AddScoped<IHttpClientService, HttpClientService>();
                    services.AddScoped(typeof(IDataEqualityComparer<>), typeof(DataEqualityComparer<>));
                    //services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
                    services.AddScoped(typeof(IApplicationRepository<>), typeof(ApplicationRepository<>));
                    services.AddScoped(typeof(ISAPRepository<>), typeof(SAPRepository<>));
                    services.AddScoped<IUnitOfWork, ApplicationDbContext>();
                    services.AddScoped<IWorkerSyncService, WorkerSyncService>();
                    services.AddScoped<IODataService, ODataService>();
                    services.AddScoped<ISyncService, SyncService>();
                    services.AddScoped<IOccasionToCelebrateService, OccasionToCelebrateService>();
                    services.AddScoped<ICreditLimitCrossNotifictionService, CreditLimitCrossNotifictionService>();
                    services.AddScoped<IChequeBounceNotificationService, ChequeBounceNotificationService>();
                    services.AddScoped<IPaymentFollowupService, PaymentFollowupService>();
                    services.AddScoped<IAlertNotificationDataService, AlertNotificationDataService>();
                    services.AddScoped<IAlertNotificationODataService, AlertNotificationODataService>();
                    services.AddScoped<INotificationWorkerService, NotificationWorkerService>();
                    services.AddHostedService<Worker>();
                    services.AddHostedService<DailySalesNTargetDataWorker>();
                    services.AddHostedService<DailySalesDataSummaryUpdateWorker>();
                    services.AddHostedService<DailyAlertNotificationBulkUploadWorker>();
                });
    }
}
