using SensorCourier.App.Models;
using SensorCourier.App.Services;

namespace SensorCourier.App;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceProvider _serviceProvider;

    public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
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
            Task[] tasks = null;
            try
            {
                // Create scope for ParameterService
                using (var scope = _serviceProvider.CreateScope())
                {
                    var parameterService = scope.ServiceProvider.GetRequiredService<ParameterService>();
                    // Get parameters from the database: BatchDelaySeconds, BatchSize, LastDateTime and Monitor.
                    // ParameterService throws an exception if parameters cannot be loaded.
                    appSettings = await parameterService.GetAppSettings(stoppingToken);
                    lastMeasurementTime = await parameterService.GetLastMeasurementTimeStamp(stoppingToken);
                    lastMetadataTime = await parameterService.GetLastMetadataTimeStamp(stoppingToken);
                    batchDelaySeconds = appSettings.BatchDelaySeconds;
                }

                _logger.LogInformation("Starting batch of size {batchSize} processing at {time}.", appSettings.BatchSize, DateTime.Now);

                // Create scope1 for the ETLService
                using var scope1 = _serviceProvider.CreateScope();
                var etlService1 = scope1.ServiceProvider.GetRequiredService<ETLService>();
                var etlTask1 = etlService1.ExtractAndLoadMeasurements(lastMeasurementTime, appSettings.BatchSize, stoppingToken);

                // Create scope2 for the ETLService
                using var scope2 = _serviceProvider.CreateScope();
                var etlService2 = scope2.ServiceProvider.GetRequiredService<ETLService>();
                var etlTask2 = etlService2.ExtractAndLoadMetadatas(lastMetadataTime, appSettings.BatchSize, stoppingToken);

                tasks = new[] {etlTask1, etlTask2};
                // Wait for both ETL tasks to complete
                await Task.WhenAll(etlTask1, etlTask2);

                _logger.LogInformation("Data processed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing data.");
                foreach (var task in tasks)
                {
                    if (task.IsFaulted)
                        _logger.LogError(task.Exception.Flatten(), "An error occurred while processing data.");
                }
            }

            _logger.LogInformation("Next batch in {batchDelay} seconds at {time}.", TimeSpan.FromSeconds(batchDelaySeconds), DateTime.Now + TimeSpan.FromSeconds(batchDelaySeconds));

            await Task.Delay(TimeSpan.FromSeconds(batchDelaySeconds), stoppingToken);
        }
    }

    //private async Task RunTask<T>(Func<T, Task> action)
    //{
    //    try
    //    {
    //        using var scope = _serviceProvider.CreateScope();
    //        var service = scope.ServiceProvider.GetRequiredService<T>();
    //        await action(service);
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex, "An error occurred while executing task.");
    //    }
    //}
}
