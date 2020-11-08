using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Berger.Data.Common;
using Berger.Data.MsfaEntity;
using Berger.Worker.Common;
using Berger.Worker.Services;
using BergerMsfaApi.Repositories;
using Coravel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace Berger.Worker
{
    public class Program
    {
        
        public static void Main(string[] args)
        {
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole()
                    .AddEventLog();
                    
            });
            ILogger logger = loggerFactory.CreateLogger<Program>();
            
            try
            {
                IHost host = CreateHostBuilder(args).Build();
                //host.Services.UseScheduler(scheduler => {
                //    scheduler
                //        .Schedule<Worker>()
                //        .EveryFiveMinutes();
                //});
                host.Run();
            }
            catch (Exception ex)
            {
                logger.LogInformation($"{ex.Message}");
            }
            
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                //.UseConsoleLifetime()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(hostContext.Configuration.GetConnectionString(nameof(ApplicationDbContext))));

                    services.AddScoped<DbContext, ApplicationDbContext>();
                    services.AddScoped<ICustomerService, CustomerService>();
                    services.AddScoped<IHttpClientService, HttpClientService>();
                    services.AddScoped(typeof(IDataEqualityComparer<>), typeof(DataEqualityComparer<>));
                    services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
                    services.AddScoped<IUnitOfWork, ApplicationDbContext>();
                    services.AddScheduler();

                    services.AddHostedService<Worker>();
                    
                    
                })
        ;
    }
}
