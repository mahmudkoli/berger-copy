using Berger.Odata.Services;
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
        private readonly IAlertNotificationDataService _alertNotification;

        public Worker(ILogger<Worker> logger, IAlertNotificationDataService alertNotification)
        {
            _logger = logger;
            _alertNotification = alertNotification;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
               var res=await _alertNotification.GetAllTodayCustomerOccasionsByDealerIds();
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
