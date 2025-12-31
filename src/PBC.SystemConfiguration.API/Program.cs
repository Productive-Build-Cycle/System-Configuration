using Microsoft.EntityFrameworkCore;
using PBC.SystemConfiguration.Application.Interfaces;
using PBC.SystemConfiguration.Infrastructure.Persistence;

public partial class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();

        // DbContext
        builder.Services.AddDbContext<ProgramDbContext>(options =>
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection"),
                sql => sql.MigrationsAssembly("PBC.SystemConfiguration.Infrastructure")
            )
        );

        // DI for Repository & Service
        builder.Services.AddScoped<FeatureFlagRepository, FeatureFlagRepository>();
        builder.Services.AddScoped<IFeatureFlagService, FeatureFlagService>();

        // OpenAPI / Swagger
        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}