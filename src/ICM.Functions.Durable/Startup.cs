using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using ICM.Functions.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Microsoft.Extensions.Logging.Abstractions;

[assembly: FunctionsStartup(typeof(Namespace.Startup))]

namespace Namespace
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

            var context = builder.GetContext();

            builder.Services.AddInfrastructureServices(context.Configuration, NullLoggerFactory.Instance);
        }
    }
}