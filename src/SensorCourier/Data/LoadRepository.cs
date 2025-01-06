using SensorCourier.Models;

namespace SensorCourier.App.Data;

/// <summary>
/// Repository for loading data in to the database.
/// </summary>
public class LoadRepository
{
    private readonly TargetDbContext _dbContext;

    public LoadRepository(TargetDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Load measurements in to the database.
    /// </summary>
    public async Task LoadMeasurements(IEnumerable<SensorMeasurement> measurements, CancellationToken cancellationToken)
    {
        _dbContext.SensorMeasurements.AddRange(measurements);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Load metadata in to the database.
    /// </summary>
    public async Task LoadMetadatas(IEnumerable<SensorMetadata> metadatas, CancellationToken cancellationToken)
    {
        _dbContext.SensorMetadata.AddRange(metadatas);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
