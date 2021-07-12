using Berger.Common.HttpClient;
using Berger.Odata.Services;
using BergerMsfaApi.Services.AlertNotification;
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
                    services.AddTransient<IHttpClientService, HttpClientService>();
                    services.AddTransient<IODataService, ODataService>();
                    services.AddTransient<IAlertNotificationDataService, AlertNotificationDataService>();
                    services.AddTransient<INotificationWorkerService, NotificationWorkerService>();
                    services.AddHostedService<Worker>();
                });
    }
}
