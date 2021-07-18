using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Berger.Worker.Model;
using Berger.Worker.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Berger.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private  ICustomerService _customerService;
        private  IBrandService _brandService;
        private  IBrandFamilyService _brandFamilyService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IOptions<WorkerConfig> _appSettings;
        private readonly int _timeOutHours = 24;
        private string workerName = "DailyCustomerBrandData Worker";

        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider, IOptions<WorkerConfig> appSettings)
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
            _logger.LogInformation($"{workerName} disposed at: {DateTime.Now}");
            base.Dispose();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                if (_appSettings.Value.RunDailyCustomerBrandDataWorker)
                {
                    try
                    {
                        _logger.LogInformation("{workerName} running at: {time}", workerName, DateTimeOffset.Now);

                        using (var scope = _serviceProvider.CreateScope())
                        {
                            _customerService = scope.ServiceProvider.GetRequiredService<ICustomerService>();
                            _brandService = scope.ServiceProvider.GetRequiredService<IBrandService>();
                            _brandFamilyService = scope.ServiceProvider.GetRequiredService<IBrandFamilyService>();

                            await _brandFamilyService.GetBrandFamilyData();
                            await _brandService.GetBrandData();
                            await _customerService.GetCustomerData();
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

                DateTime nextRunTime = GetHourIntervals(clockIn, clockOut, _timeOutHours).FirstOrDefault(x => x > today);

                if (nextRunTime.Hour == 23 && nextRunTime.Minute == 59)
                {
                    nextRunTime = nextRunTime.AddMinutes(1);
                }

                TimeSpan actualTime = nextRunTime - today;
                _logger.LogInformation($"{workerName} ______Next Service will run after: {actualTime}");

                await Task.Delay(actualTime, stoppingToken);
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
}
