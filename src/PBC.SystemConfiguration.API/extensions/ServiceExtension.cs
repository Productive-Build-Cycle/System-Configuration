using PBC.SystemConfiguration.Infrastructure;

namespace PBC.SystemConfiguration.API.Extensions;

public static class ServiceExtension
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services
            .AddInfrastructureServices();

        return services;
    }
}