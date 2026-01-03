using PBC.SystemConfiguration.API.Extensions;
using PBC.SystemConfiguration.API.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args)
    .AddSerilog();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureServices(builder.Configuration);
builder.Services.AddApisVersioning();
builder.Services.AddSwagger();
builder.Services.ConfigureApiBehavior();
builder.Services.AddMemoryCache();

var app = builder.Build();

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseAuthorization();

app.UseRequestBodyLogging();
app.UseSerilogRequestLogging(option => { option.IncludeQueryInRequestPath = true; });

app.MapControllers();

app.Run();