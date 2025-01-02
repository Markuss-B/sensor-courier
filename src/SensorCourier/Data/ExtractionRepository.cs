using MongoDB.Driver;
using SensorCourier.App.Models;

namespace SensorCourier.App.Data;

/// <summary>
/// Repository for extracting data from mongo db.
/// </summary>
public class ExtractionRepository
{
    private readonly MongoDb _db;

    public ExtractionRepository(MongoDb db)
    {
        _db = db;
    }

    /// <summary>
    /// Extract sensor metadata <see cref="MongoSensorMetadatas"/> from mongo db.
    /// </summary>
    /// <param name="lastDateTime">Date from which to fetch data.</param>
    /// <param name="batchSize">How many documents to fetch.</param>
    /// <returns></returns>
    public async Task<IEnumerable<MongoSensorMetadatas>> ExtractSensorMetadatas(DateTime lastDateTime, int batchSize, CancellationToken cancellationToken)
    {
        return await _db.SensorMetadatas
            .Find(sm => sm.Timestamp > lastDateTime)
            .SortBy(sm => sm.Timestamp)
            .Limit(batchSize)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Extract sensor measurements <see cref="MongoSensorMeasurements"/> from mongo db.
    /// </summary>
    /// <param name="lastDateTime">Date from which to fetch data.</param>
    /// <param name="batchSize">How many documents to fetch.</param>
    /// <returns></returns>
    public async Task<IEnumerable<MongoSensorMeasurements>> ExtractSensorMeasurements(DateTime lastDateTime, int batchSize, CancellationToken cancellationToken)
    {
        return await _db.SensorMeasurements
            .Find(sm => sm.Timestamp > lastDateTime)
            .SortBy(sm => sm.Timestamp)
            .Limit(batchSize)
            .ToListAsync(cancellationToken);
    }
}
