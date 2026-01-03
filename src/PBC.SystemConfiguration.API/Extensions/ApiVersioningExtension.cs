using Asp.Versioning;

namespace PBC.SystemConfiguration.API.Extensions;

/// <summary>
/// Provides an extension method to configure API versioning for the application.
/// </summary>
public static class ApiVersioningExtension
{
    /// <summary>
    /// Configures API versioning and versioned API exploration for the application.
    /// </summary>
    /// <param name="services">
    /// The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> 
    /// to which API versioning services are added.
    /// </param>
    public static void AddApisVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = ApiVersion.Default;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });
    }
}