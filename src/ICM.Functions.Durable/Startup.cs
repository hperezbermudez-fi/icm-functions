using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using ICM.Functions.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;
using ICM.Functions.Durable;

[assembly: FunctionsStartup(typeof(Startup))]

namespace ICM.Functions.Durable
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddLogging(builder =>
            {
                builder.AddApplicationInsights();
                builder.AddFilter<ApplicationInsightsLoggerProvider>(string.Empty, LogLevel.Trace);
            });

            builder.Services.AddInfrastructureServices();
        }
    }
}