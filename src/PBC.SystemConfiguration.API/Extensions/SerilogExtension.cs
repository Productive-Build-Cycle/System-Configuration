using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

namespace PBC.SystemConfiguration.API.Extensions;

public static class SerilogExtension
{
    public static WebApplicationBuilder AddSerilog(this WebApplicationBuilder builder)
    {
         // Get the connection string for Serilog from configuration
        var serilogDbConnection = builder.Configuration.GetConnectionString("DefaultConnection");

        // These options map Serilog properties to SQL table columns.
        var columnOptions = new ColumnOptions();
        columnOptions.Store.Remove(StandardColumn.Properties); // Remove default Properties column
        columnOptions.Store.Add(StandardColumn.LogEvent); // Keep the full LogEvent JSON
        columnOptions.Store.Remove(StandardColumn.MessageTemplate);

        columnOptions.AdditionalColumns = new List<SqlColumn>
        {
            new()
            {
                ColumnName = "SourceContext",
                DataType = System.Data.SqlDbType.NVarChar,
                DataLength = 255, // Or a suitable length for your source contexts
                AllowNull = true
            },
            new()
            {
                ColumnName = "RequestBody",
                DataType = System.Data.SqlDbType.NVarChar, // NVarChar(MAX) or similar depending on expected body size
                DataLength = -1, // -1 for NVARCHAR(MAX)
                AllowNull = true
            }
        };

        // Create an instance of MSSqlServerSinkOptions
        var sinkOptions = new MSSqlServerSinkOptions
        {
            TableName = "Logs",
            AutoCreateSqlTable = true,
            SchemaName = "log",
            BatchPostingLimit = 20,
            BatchPeriod = TimeSpan.FromSeconds(5)
        };

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration) // Read base Serilog config from appsettings
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.MSSqlServer(
                connectionString: serilogDbConnection,
                sinkOptions: sinkOptions,
                columnOptions: columnOptions,
                restrictedToMinimumLevel: LogEventLevel.Information
            )
            .CreateLogger();

        builder.Host.UseSerilog();

        return builder;
    }
}