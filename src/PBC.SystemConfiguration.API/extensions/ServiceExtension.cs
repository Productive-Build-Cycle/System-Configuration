using PBC.SystemConfiguration.Infrastructure;

namespace PBC.SystemConfiguration.API.Extensions;

public static class ServiceExtension
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddInfrastructureServices();

        return services;
    }
}