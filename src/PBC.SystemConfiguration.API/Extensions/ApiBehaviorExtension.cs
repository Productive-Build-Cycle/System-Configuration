using Microsoft.AspNetCore.Mvc;
using PBC.SystemConfiguration.Application.Dtos.Common;

namespace PBC.SystemConfiguration.API.Extensions;

public static class ApiBehaviorExtension
{
    public static IServiceCollection ConfigureApiBehavior(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage).ToList();
                return new BadRequestObjectResult(new Response<object>(400, "Validation failed.", false, errors));
            };
        });

        return services;
    }
}