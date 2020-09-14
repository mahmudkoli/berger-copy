using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace BergerMsfaApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
            //.UseUrls("http://localhost:7800","https://localhost:7801");
    }
}
