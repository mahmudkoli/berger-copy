using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Berger.Worker.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Berger.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private  ICustomerService _customerService;
        private  IBrandService _brandService;
        private  IBrandFamilyService _brandFamilyService;
        private readonly IServiceProvider _serviceProvider;
        private readonly int _timeOutHours = 24;

        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Worker started at {DateTime.Now}");
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Worker stopped at {DateTime.Now}");
            return base.StopAsync(cancellationToken);
        }

        public override void Dispose()
        {
            _logger.LogInformation($"Worker disposed at: {DateTime.Now}");
            base.Dispose();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                try
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

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
                    _logger.LogError(ex, $"Failed to update all data.");
                }
                finally
                {
                    stopwatch.Stop();
                }

                TimeSpan actualTime = TimeSpan.FromHours(_timeOutHours) - stopwatch.Elapsed;
                _logger.LogInformation($"______Next Service will run after: {actualTime}");

                await Task.Delay(actualTime, stoppingToken);
            }
        }
    }
}
