using Microsoft.Extensions.DependencyInjection;
using PBC.SystemConfiguration.Application.Interfaces;
using PBC.SystemConfiguration.Application.Services;

namespace PBC.SystemConfiguration.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAppSettingService, AppSettingService>();
        services.AddScoped<IFeatureFlagService, FeatureFlagService>();
        
        return services;
    }
}