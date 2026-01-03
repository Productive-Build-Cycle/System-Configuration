using Microsoft.EntityFrameworkCore;
using PBC.SystemConfiguration.API.Extensions;
using PBC.SystemConfiguration.Infrastructure.Persistence.DbContext;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.ConfigureServices(builder.Configuration);


var app = builder.Build();

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();