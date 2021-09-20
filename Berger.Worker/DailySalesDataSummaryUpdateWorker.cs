using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Berger.Data.MsfaEntity.SAPReports;
using Berger.Data.MsfaEntity.Sync;
using Berger.Worker.Model;
using Berger.Worker.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Berger.Worker
{
    public class DailySalesDataSummaryUpdateWorker : BackgroundService
    {
        private readonly ILogger<DailySalesDataSummaryUpdateWorker> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IOptions<WorkerConfig> _appSettings;
        private string workerName = "DailySalesDataSummaryUpdate Worker";
        private ISAPRepository<QuarterlyPerformanceReport> _sapRepository;
        private readonly int _startTimeInMinutes = 30;

        public DailySalesDataSummaryUpdateWorker(
            ILogger<DailySalesDataSummaryUpdateWorker> logger, 
            IServiceProvider serviceProvider, 
            IOptions<WorkerConfig> appSettings)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _appSettings = appSettings;
            _startTimeInMinutes = _appSettings.Value.DailySalesDataSummaryUpdateStartTimeInMinute;
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
                var today = DateTime.Now;

                if (_appSettings.Value.RunDailySalesDataSummaryUpdateWorker)
                {
                    try
                    {
                        _logger.LogInformation("{workerName}  running at: {time}", workerName, DateTimeOffset.Now);

                        using (var scope = _serviceProvider.CreateScope())
                        {
                            _sapRepository = scope.ServiceProvider.GetRequiredService<ISAPRepository<QuarterlyPerformanceReport>>();
                            await _sapRepository.ExecuteSqlCommandAsync("spMsfaSalesDataAllSummaryUpdate");
                            _logger.LogInformation("{workerName}  updated data at: {time}", workerName, DateTimeOffset.Now);
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

                DateTime nextRunTime = new DateTime(today.Year, today.Month, today.Day, today.Hour + 1, _startTimeInMinutes, 01);
                TimeSpan actualTime = nextRunTime - today;
                _logger.LogInformation($"{workerName} ______Next Service will run after: {actualTime}");

                await Task.Delay(actualTime, stoppingToken);
            }
        }
    }
}
