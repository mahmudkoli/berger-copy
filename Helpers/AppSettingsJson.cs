using System.IO;
using Microsoft.Extensions.Configuration;

namespace BergerMsfaApi.Helpers
{
    public static class AppSettingsJson
    {
        public static string ConnectionString
        {
            get
            {
                var config = GetAppSettings();
                return config.GetConnectionString("ApplicationDbContext");
            }
        }

        public static IConfigurationRoot GetAppSettings()
        {
            string applicationExeDirectory = Directory.GetCurrentDirectory();

            var builder = new ConfigurationBuilder()
            .SetBasePath(applicationExeDirectory)
            .AddJsonFile("appsettings.json");

            return builder.Build();
        }


    }
}
