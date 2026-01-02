
using Microsoft.EntityFrameworkCore;
using PBC.SystemConfiguration.API.Extensions;
using PBC.SystemConfiguration.Domain.Interfaces;
using PBC.SystemConfiguration.Infrastructure.Persistence;
using PBC.SystemConfiguration.Infrastructure.Persistence.DbContext;
using PBC.SystemConfiguration.Infrastructure.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.ConfigureServices();
builder.Services.AddDbContext<ProgramDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sql => sql.MigrationsAssembly("PBC.SystemConfiguration.Infrastructure") 
    )
);


builder.Services.AddScoped<IFeatureFlagRepository, FeatureFlagRepository>();
//IServiceCollection serviceCollection = builder.Services.AddScoped<IFeatureFlagService, FeatureFlagService>();


builder.Services.AddOpenApi();

var app = builder.Build();

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();