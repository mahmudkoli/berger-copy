using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Berger.Data.MsfaEntity.Sync;
using Berger.Worker.Model;
using Berger.Worker.Services;
using Berger.Worker.Services.AlertNotification;
using BergerMsfaApi.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Berger.Worker
{
    public class DailyAlertNotificationBulkUploadWorker : BackgroundService
    {
        private readonly ILogger<DailyAlertNotificationBulkUploadWorker> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IOptions<WorkerConfig> _appSettings;
        private INotificationWorkerService _notificationWorker;
        private string workerName = "DailyAlertNotificationBulkUpload Worker";

        private IRepository<SyncSetup> _repository;
        public DailyAlertNotificationBulkUploadWorker(ILogger<DailyAlertNotificationBulkUploadWorker> logger, IServiceProvider serviceProvider, IOptions<WorkerConfig> appSettings)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _appSettings = appSettings;
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{workerName} started at {DateTime.Now}");
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{workerName} stopped at {DateTime.Now}");
            return base.StopAsync(cancellationToken);
        }

        public override void Dispose()
        {
            _logger.LogInformation($"{workerName}  disposed at: {DateTime.Now}");
            base.Dispose();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                if (_appSettings.Value.RunDailyAlertNotificationDataWorker)
                {
                    try
                    {
                        _logger.LogInformation("{workerName}  running at: {time}", workerName, DateTimeOffset.Now);

                        using (var scope = _serviceProvider.CreateScope())
                        {
                            _notificationWorker = scope.ServiceProvider.GetRequiredService<INotificationWorkerService>();
                            //await _notificationWorker.SavePaymnetFollowup();
                            //await _notificationWorker.SaveOccassionToCelebrste();
                            await _notificationWorker.SaveCheckBounceNotification();
                            //await _notificationWorker.SaveCreaditLimitNotification();
                        }

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"{workerName} Failed to update all data.");
                    }
                    finally
                    {
                        stopwatch.Stop();
                    }
                }


                var today = DateTime.Now;
                var clockOut = new DateTime(today.Year, today.Month, today.Day, 23, 59, 0);
                var clockIn = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0);
                var syncSetup = new SyncSetup();

                using (var scope = _serviceProvider.CreateScope())
                {
                    _repository = scope.ServiceProvider.GetRequiredService<IRepository<SyncSetup>>();
                    syncSetup = await _repository.FirstOrDefaultAsync(x => true);
                }


                DateTime nextRunTime = GetHourIntervals(clockIn, clockOut, syncSetup.SyncHourlyInterval).FirstOrDefault(x => x > today);

                if (nextRunTime.Hour == 23 && nextRunTime.Minute == 59)
                {
                    nextRunTime = nextRunTime.AddMinutes(1);
                }

                TimeSpan actualTime = nextRunTime - today;
                _logger.LogInformation($"{workerName} ______Next Service will run after: {actualTime}");

                await Task.Delay(actualTime, stoppingToken);
            }
        }


        IEnumerable<DateTime> GetHourIntervals(DateTime clockIn, DateTime clockOut, int hourlyDelay)
        {
            yield return clockIn;

            DateTime d = new DateTime(clockIn.Year, clockIn.Month, clockIn.Day, clockIn.Hour, 0, 0, clockIn.Kind).AddHours(hourlyDelay);

            while (d < clockOut)
            {
                yield return d;
                d = d.AddHours(hourlyDelay);
            }

            yield return clockOut;
        }
    }
}
