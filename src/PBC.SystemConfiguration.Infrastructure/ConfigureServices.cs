using Microsoft.Extensions.DependencyInjection;
using PBC.SystemConfiguration.Domain.Interfaces;
using PBC.SystemConfiguration.Infrastructure.Persistence.Repositories;

namespace PBC.SystemConfiguration.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IAppSettingRepository, AppSettingRepository>();
        services.AddScoped<IFeatureFlagRepository, FeatureFlagRepository>();
        
        return services;
    }
}