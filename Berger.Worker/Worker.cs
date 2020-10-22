using System;
using System.Threading;
using System.Threading.Tasks;
using Berger.Worker.Services;
using Coravel.Invocable;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Berger.Worker
{
    public class Worker : BackgroundService, IInvocable
    {
        private readonly ILogger<Worker> _logger;
        private  ICustomerService _customerService;
        private readonly IServiceProvider _serviceProvider;
/*
        private readonly Timer _timer;
*/

        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            
            _logger.LogInformation("Worker Started {date}", DateTime.Now);
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Worker Stopped {date}", DateTime.Now);
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                try
                { 
                    _customerService = scope.ServiceProvider.GetRequiredService<ICustomerService>();
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    await _customerService.getData();
                }
                catch (Exception ex)
                {
                    _logger.LogCritical($"{ex.Message}");
                }
                await Task.Delay(5000, stoppingToken);
            }
        }

        public Task Invoke()
        {
            StartAsync(cancellationToken: CancellationToken.None);
            return Task.CompletedTask;
        }
    }
}
