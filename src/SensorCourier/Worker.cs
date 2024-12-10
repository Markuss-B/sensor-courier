using SensorCourier.App.Services;

namespace SensorCourier.App
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly SensorService _sensorService;

        public Worker(ILogger<Worker> logger, SensorService sensorService)
        {
            _logger = logger;
            _sensorService = sensorService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Get parameters from the database: BatchDelaySeconds, BatchSize, LastDateTime and Monitor
                DateTime lastDateTime = DateTime.Now;
                // Get sensor metadata by LastDateTime
                var sensors = await _sensorService.GetMongoSensorsAsync(lastDateTime);
                if (sensors
                // Save sensor metadata to the database
                // Get sensor measurements by LastDateTime
                // Save sensor measurements to the database
                // Update LastDateTime in the database
                // Wait for BatchDelaySeconds
            }
        }
    }
}
