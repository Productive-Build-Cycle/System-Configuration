using Asp.Versioning.ApiExplorer;
using Microsoft.OpenApi;

namespace PBC.SystemConfiguration.API.Extensions;

public static class SwaggerExtension
{
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, new OpenApiInfo
                {
                    Title = $"System Configuration API {description.ApiVersion}",
                    Version = description.ApiVersion.ToString(),
                    Description = "API documentation for the System Configuration."
                });
            }
        });
    }
}