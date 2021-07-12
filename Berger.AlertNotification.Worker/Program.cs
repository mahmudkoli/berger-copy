using Berger.Common.HttpClient;
using Berger.Data.Common;
using Berger.Data.MsfaEntity;
using Berger.Odata.Services;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.AlertNotification;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Berger.AlertNotification.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    //services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(hostContext.Configuration.GetConnectionString(nameof(ApplicationDbContext))));
                    //services.AddScoped<DbContext, ApplicationDbContext>();
                    services.AddScoped<IHttpClientService, HttpClientService>();
                    services.AddScoped<IODataService, ODataService>();
                    services.AddScoped<IAlertNotificationODataService, AlertNotificationODataService>();
                    services.AddScoped<IOccasionToCelebrateService, OccasionToCelebrateService>();
                    services.AddScoped<ICreditLimitCrossNotifictionService, CreditLimitCrossNotifictionService>();
                    services.AddScoped<IChequeBounceNotificationService, ChequeBounceNotificationService>();
                    services.AddScoped<IPaymentFollowupService, PaymentFollowupService>();
                    services.AddScoped<IAlertNotificationDataService, AlertNotificationDataService>();
                    services.AddScoped<INotificationWorkerService, NotificationWorkerService>();
                    services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
                    //services.AddScoped<IUnitOfWork, ApplicationDbContext>();
                    services.AddHostedService<Worker>();
                });
    }
}
