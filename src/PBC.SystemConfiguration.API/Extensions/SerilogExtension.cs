using Microsoft.Data.SqlClient;
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

        var loggerConfiguration = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .WriteTo.Console();

        if (CanConnectToDatabase(serilogDbConnection))
        {
            // These options map Serilog properties to SQL table columns.
            var columnOptions = new ColumnOptions();
            columnOptions.Store.Remove(StandardColumn.Properties); // Remove default Properties column
            columnOptions.Store.Add(StandardColumn.LogEvent); // Keep the full LogEvent JSON
            columnOptions.Store.Remove(StandardColumn.MessageTemplate);

            columnOptions.AdditionalColumns = new List<SqlColumn>
            {
                new() { ColumnName = "SourceContext", DataType = System.Data.SqlDbType.NVarChar, DataLength = 255, AllowNull = true },
                new() { ColumnName = "RequestBody", DataType = System.Data.SqlDbType.NVarChar, DataLength = -1, AllowNull = true }
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

            loggerConfiguration.WriteTo.MSSqlServer(
                connectionString: serilogDbConnection,
                sinkOptions: sinkOptions,
                columnOptions: columnOptions,
                restrictedToMinimumLevel: LogEventLevel.Information
            );
        }

        Log.Logger = loggerConfiguration.CreateLogger();
        builder.Host.UseSerilog();

        return builder;
    }

    private static bool CanConnectToDatabase(string? connectionString)
    {
        if (string.IsNullOrEmpty(connectionString)) return false;

        try
        {
            using var connection = new SqlConnection(connectionString);
            connection.Open();
            return true;
        }
        catch
        {
            return false;
        }
    }
}