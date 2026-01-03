using PBC.SystemConfiguration.Application;
using PBC.SystemConfiguration.Infrastructure;

namespace PBC.SystemConfiguration.API.Extensions;

public static class ServiceExtension
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services
            .AddApplicationServices()
            .AddInfrastructureServices();

        return services;
    }
}