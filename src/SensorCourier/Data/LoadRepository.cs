using SensorCourier.Models;

namespace SensorCourier.App.Data;

public class LoadRepository
{
    private readonly TargetDbContext _dbContext;

    public LoadRepository(TargetDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task LoadMeasurements(IEnumerable<SensorMeasurement> measurements, CancellationToken cancellationToken)
    {
        _dbContext.SensorMeasurements.AddRange(measurements);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task LoadMetadatas(IEnumerable<SensorMetadata> metadatas, CancellationToken cancellationToken)
    {
        _dbContext.SensorMetadata.AddRange(metadatas);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
