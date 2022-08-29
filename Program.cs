using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;

namespace WebJob.Sample.Listner
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var host1 = Host.CreateDefaultBuilder(args)
                .ConfigureWebJobSettings()
                .ConfigureLogging()
                .ConfigureDependencyInjectionServices()
                .Build();

            await host1.RunAsync();
        }
    }

    public static class WebJobExtensions
    {
        public static IHostBuilder ConfigureWebJobSettings(this IHostBuilder builder)
        {
            builder.ConfigureWebJobs(webHostBuilder =>
            {
                webHostBuilder.AddAzureStorageCoreServices();
                webHostBuilder.AddServiceBus(option =>
                {
                    option.MaxConcurrentCalls = 10;
                    option.AutoCompleteMessages = true;

                });
            });

            return builder;
        }

        public static IHostBuilder ConfigureLogging(this IHostBuilder builder)
        {
            builder.ConfigureLogging((context, loggingBuilder) =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddConsole();
                loggingBuilder.SetMinimumLevel(LogLevel.Warning);

                // If the key exists in settings, use it to enable Application Insights.
                var instrumentationKey = context.Configuration["ApplicationInsights:ConnectionString"];
                if (!string.IsNullOrEmpty(instrumentationKey))
                {
                    loggingBuilder.AddApplicationInsightsWebJobs(o => o.ConnectionString = instrumentationKey);
                }

            });

            return builder;
        }

        public static IHostBuilder ConfigureDependencyInjectionServices(this IHostBuilder builder)
        {
            builder.ConfigureServices((hostBuildContext, services) =>
            {
                var configuration = hostBuildContext.Configuration;
                services.AddPersistence(configuration);
                services.AddApplication(configuration);
            });

            return builder;
        }
    }
}
