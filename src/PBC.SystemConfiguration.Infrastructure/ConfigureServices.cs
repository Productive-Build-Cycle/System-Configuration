using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PBC.SystemConfiguration.Domain.Interfaces;
using PBC.SystemConfiguration.Infrastructure.Persistence.DbContext;
using PBC.SystemConfiguration.Infrastructure.Persistence.Repositories;

namespace PBC.SystemConfiguration.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IAppSettingRepository, AppSettingRepository>();
        services.AddScoped<IFeatureFlagRepository, FeatureFlagRepository>();

        services.AddDbContext<ProgramDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                assembly => assembly.MigrationsAssembly("PBC.SystemConfiguration.Infrastructure")
            )
        );

        return services;
    }
}