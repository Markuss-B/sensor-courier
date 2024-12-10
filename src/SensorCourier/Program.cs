using Microsoft.EntityFrameworkCore;
using SensorCourier.App;
using SensorCourier.App.Data;
using SensorCourier.App.Services;
using SensorCourier.Models;
using SensorCourier.MySql.Extensions;
using static SensorCourier.App.Provider;

var builder = Host.CreateApplicationBuilder(args);

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables()
    .AddCommandLine(args)
    .Build();

// Database based on the provider specified by "Provider" variable from appsettings.json, command line or environment variables.
builder.Services.AddDbContext<TargetDbContext>(options =>
{
    var provider = config.GetValue("Provider", SqlServer.Name);
    
    // SqlServer
    if (provider == SqlServer.Name)
    {
        options.UseSqlServer(
            config.GetConnectionString(SqlServer.Name)!,
            x => x.MigrationsAssembly(SqlServer.Assembly)
        );
    }
    // MySql
    else if (provider == MySql.Name)
    {
        options.UseMySql(
            config.GetConnectionString(MySql.Name)!,
            MySqlServerVersion.AutoDetect(config.GetConnectionString(MySql.Name)!),
            x => x.MigrationsAssembly(MySql.Assembly)
        );
        options.UseMySqlExtensions();
    }   
});

// Services
builder.Services.AddScoped<ExtractionRepository>();
builder.Services.AddScoped<LoadRepository>();
builder.Services.AddScoped<ETLService>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
using var scope = host.Services.CreateScope();
var services = scope.ServiceProvider;

// Logger
var logger = services.GetRequiredService<ILogger<Program>>();

// Check if the database is available.
try
{
    logger.LogInformation("Attempting to connect to the database.");

    var db = services.GetRequiredService<TargetDbContext>();
    db.Database.CanConnect();
    logger.LogInformation("Successfully connected to the database.");

    await TargetDbContext.InitializeAsync(db, logger);

    logger.LogInformation("Database is usable.");
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occurred while connecting to the database.");
    return;
}

host.Run();
