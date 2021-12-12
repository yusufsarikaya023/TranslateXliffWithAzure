using Microsoft.Extensions.Configuration;
using static System.IO.Directory;

namespace Translation;

public static class Configuration
{
    public static IConfiguration AppSettings;
    static Configuration()
    {
        AppSettings = new ConfigurationBuilder()
        .SetBasePath(GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .AddEnvironmentVariables()
        .Build();
    }
}