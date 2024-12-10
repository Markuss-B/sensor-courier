using SensorCourier.App.Models;
using SensorCourier.App.Services;

namespace SensorCourier.App;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly ParameterService _parameterService;
    private readonly IServiceProvider _serviceProvider;

    public Worker(ILogger<Worker> logger, ParameterService parameterService, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _parameterService = parameterService;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        AppSettings appSettings;
        double batchDelaySeconds = 100;
        DateTime lastMeasurementTime;
        DateTime lastMetadataTime;

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Get parameters from the database: BatchDelaySeconds, BatchSize, LastDateTime and Monitor.
                // ParameterService throws an exception if parameters cannot be loaded.
                appSettings = _parameterService.GetAppSettings();
                batchDelaySeconds = appSettings.BatchDelaySeconds;
                lastMeasurementTime = _parameterService.GetLastMeasurementTimeStamp();
                lastMetadataTime = _parameterService.GetLastMetadataTimeStamp();

                _logger.LogInformation("Processing data from {lastMeasurementTime} and {lastMetadataTime}.", lastMeasurementTime, lastMetadataTime);

                // Create scope1 for the ETL service
                using var scope1 = _serviceProvider.CreateScope();
                var etlService1 = scope1.ServiceProvider.GetRequiredService<ETLService>();
                var etlTask1 = etlService1.ExtractAndLoadMeasurements(lastMeasurementTime, appSettings.BatchSize, stoppingToken);

                // Create scope2 for the ETL service
                using var scope2 = _serviceProvider.CreateScope();
                var etlService2 = scope2.ServiceProvider.GetRequiredService<ETLService>();
                var etlTask2 = etlService2.ExtractAndLoadMetadatas(lastMetadataTime, appSettings.BatchSize, stoppingToken);

                // Wait for both ETL tasks to complete
                await Task.WhenAll(etlTask1, etlTask2);

                _logger.LogInformation("Successfully processed data.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing data.");
            }

            _logger.LogInformation("Next batch in {batchDelay} seconds at {time}.", TimeSpan.FromSeconds(batchDelaySeconds), DateTime.Now + TimeSpan.FromSeconds(batchDelaySeconds));

            await Task.Delay(TimeSpan.FromSeconds(batchDelaySeconds), stoppingToken);
        }
    }
}
