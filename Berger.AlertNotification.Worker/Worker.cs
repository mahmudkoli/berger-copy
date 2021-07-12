using Berger.Odata.Services;
using BergerMsfaApi.Services.AlertNotification;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Berger.AlertNotification.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        //private readonly IAlertNotificationDataService _alertNotificationData;
        private readonly INotificationWorkerService _alertNotificationData;
        //private readonly IOccasionToCelebrateService _occasionToCelebrate;

        public Worker(ILogger<Worker> logger,
            //, IOccasionToCelebrateService occasionToCelebrate
            INotificationWorkerService alertNotificationData
            )
        {
            _logger = logger;
            //_occasionToCelebrate = occasionToCelebrate;
            _alertNotificationData = alertNotificationData;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _alertNotificationData.get();
                //_occasionToCelebrate.SaveOccasionToCelebrate();
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
