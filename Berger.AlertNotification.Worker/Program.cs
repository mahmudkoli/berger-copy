using AlertNotification.Worker.Repositories;
using Berger.AlertNotification.Worker.Repositories;
using Berger.Data.Common;
using Berger.Data.MsfaEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Serilog;
using Serilog.Events;
using Microsoft.Extensions.Configuration;
using Berger.Odata.Services;
using Berger.Common.HttpClient;

namespace Berger.AlertNotification.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();

            //Log.Logger = new LoggerConfiguration()
            //            .MinimumLevel.Debug()
            //            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            //            .Enrich.FromLogContext()
            //            .WriteTo.Console()
            //            .CreateLogger();

            //try
            //{
            //    Log.Information("Application Starting up");
            //    CreateHostBuilder(args).Build().Run();
            //}
            //catch (Exception ex)
            //{
            //    Log.Fatal(ex, "Application start-up failed");
            //}
            //finally
            //{
            //    Log.CloseAndFlush();
            //}
        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                //.UseWindowsService()
                //.UseSerilog()
                .ConfigureServices((hostContext, services) =>
                {
                    //services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(hostContext.Configuration.GetConnectionString(nameof(ApplicationDbContext))));
                    //services.AddScoped<DbContext, ApplicationDbContext>();
                    services.AddScoped<IAlertNotificationDataService, AlertNotificationDataService>();
                    services.AddScoped<IODataService, ODataService>();
                    //services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
                    //services.AddScoped<IUnitOfWork, ApplicationDbContext>();
                    services.AddScoped<IHttpClientService, HttpClientService>();
                    services.AddHostedService<Worker>();
                });
    }
}
