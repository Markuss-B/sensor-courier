using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SensorCourier.App.Models;

namespace SensorCourier.App.Data;

public class MongoDb : ISensorRepository
{
    public MongoDb(IOptions<MongoDbSettings> options)
    {
        MongoDbSettings settings = options.Value;
        MongoClient client = new MongoClient(settings.ConnectionString);
        IMongoDatabase database = client.GetDatabase(settings.DatabaseName);

        Sensors = database.GetCollection<MongoSensor>("sensors");
        SensorMeasurements = database.GetCollection<MongoSensorMeasurements>("sensorMeasurements");
    }

    public IMongoCollection<MongoSensor> Sensors { get; set; }
    public IMongoCollection<MongoSensorMeasurements> SensorMeasurements { get; set; }

    public async Task<IEnumerable<MongoSensor>> GetSensorsAsync(DateTime lastDateTime)
    {
        return await Sensors.Find(s => s.LastUpdated > lastDateTime)
            .ToListAsync();
    }

    public async Task<IEnumerable<MongoSensorMeasurements>> GetSensorMeasurementsAsync(DateTime lastDateTime, int batchSize)
    {
        return await SensorMeasurements.Find(m => m.Timestamp > lastDateTime)
            .SortBy(m => m.Timestamp)
            .Limit(batchSize)
            .ToListAsync();
    }
}
