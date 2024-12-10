using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SensorCourier.App.Models;

namespace SensorCourier.App.Data;

public class MongoDb
{
    public MongoDb(IOptions<MongoDbSettings> options)
    {
        MongoDbSettings settings = options.Value;
        MongoClient client = new MongoClient(settings.ConnectionString);
        IMongoDatabase database = client.GetDatabase(settings.DatabaseName);

        SensorMetadatas = database.GetCollection<MongoSensorMetadatas>("sensorMetadatas");
        SensorMeasurements = database.GetCollection<MongoSensorMeasurements>("sensorMeasurements");
    }

    public IMongoCollection<MongoSensorMetadatas> SensorMetadatas { get; set; }
    public IMongoCollection<MongoSensorMeasurements> SensorMeasurements { get; set; }

    //public async Task<IEnumerable<MongoSensorMetadatas>> GetSensorMetadatasAsync(DateTime lastDateTime)
    //{
    //    return await Sensors.Find(s => s.LastUpdated > lastDateTime)
    //        .ToListAsync();
    //}

    //public async Task<IEnumerable<MongoSensorMeasurements>> GetSensorMeasurementsAsync(DateTime lastDateTime, int batchSize)
    //{
    //    return await SensorMeasurements.Find(m => m.Timestamp > lastDateTime)
    //        .SortBy(m => m.Timestamp)
    //        .Limit(batchSize)
    //        .ToListAsync();
    //}
}
