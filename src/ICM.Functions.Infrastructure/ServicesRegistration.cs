using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ICM.Functions.Infrastructure.Services
{
    public static class ServicesRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            services.AddScoped<IEncrypter>(c => {
                var publicKeyBase64 = Environment.GetEnvironmentVariable("PgpPublicKey");

                if(string.IsNullOrEmpty(publicKeyBase64))
                {
                    throw new ArgumentException("The PgpPublicKey Environment Variable is missing");
                }

                return new PGPEncrypter(publicKeyBase64);
            });

            return services;
        }
    }
}
