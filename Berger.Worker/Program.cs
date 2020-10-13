using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Berger.Worker.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Berger.Worker
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
                    
                    services.AddSingleton<ICustomerService, CustomerService>();
                    services.AddHostedService<Worker>();
                });
    }
}
