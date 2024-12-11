using Microsoft.EntityFrameworkCore.Metadata;
using SensorCourier.App.Data;
using SensorCourier.App.Models;
using SensorCourier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorCourier.App.Services;

/// <summary>
/// Service that extracts, transforms and loads data from source db to target db.
/// </summary>
public class ETLService
{
    private readonly ILogger<ETLService> _logger;
    private readonly ExtractionRepository _extractionRepo;
    private readonly LoadRepository _loadRepo;

    public ETLService(ILogger<ETLService> logger, ExtractionRepository extractionRepository, LoadRepository loadRepository)
    {
        _logger = logger;
        _extractionRepo = extractionRepository;
        _loadRepo = loadRepository;
    }

    /// <summary>
    /// Extracts <paramref name="batchSize"/> amount of <see cref="MongoSensorMeasurements"/> after <paramref name="lastDateTime"/>,
    /// transforms them to <see cref="SensorMeasurement"/> and loads them in to target db.
    /// </summary>
    /// <param name="lastDateTime"></param>
    /// <param name="batchSize"></param>
    public async Task ExtractAndLoadMeasurements(DateTime lastDateTime, int batchSize, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Extracting measurements...");
        var measurements = await _extractionRepo.ExtractSensorMeasurements(lastDateTime, batchSize, cancellationToken);
        _logger.LogInformation("Measurements extracted. Count: {count}", measurements.Count());

        if (!measurements.Any())
        {
            _logger.LogWarning("No measurements to load.");
            return;
        }

        var transformedMeasurements = measurements.SelectMany(m => m.Measurements.Select(kvp => new SensorMeasurement
        {
            SensorId = m.SensorId,
            Timestamp = m.Timestamp,
            MetricKey = kvp.Key,
            MetricValue = kvp.Value.ToString() ?? "failed to convert"
        }));

        _logger.LogDebug("Loading measurements...");
        await _loadRepo.LoadMeasurements(transformedMeasurements, cancellationToken);
        _logger.LogInformation("Measurements loaded.");
    }

    /// <summary>
    /// Extracts <paramref name="batchSize"/> amount of <see cref="MongoSensorMetadatas"/> after <paramref name="lastDateTime"/>,
    /// transforms them to <see cref="SensorMetadata"/> and loads them in to target db.
    /// </summary>
    /// <param name="lastDateTime"></param>
    /// <param name="batchSize"></param>
    public async Task ExtractAndLoadMetadatas(DateTime lastDateTime, int batchSize, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Extracting metadatas...");
        IEnumerable<MongoSensorMetadatas> metadatas = await _extractionRepo.ExtractSensorMetadatas(lastDateTime, batchSize, cancellationToken);

        _logger.LogInformation("Metadatas extracted. Count: {count}", metadatas.Count());
        if (!metadatas.Any())
        {
            _logger.LogWarning("No metadatas to load.");
            return;
        }

        var transformedMetadatas = metadatas.SelectMany(m => m.Metadata.Select(kvp => new SensorMetadata
        {
            SensorId = m.SensorId,
            Timestamp = m.Timestamp,
            MetaKey = kvp.Key,
            MetaValue = kvp.Value.ToString() ?? "failed to convert"
        }));

        _logger.LogDebug("Loading metadatas...");

        await _loadRepo.LoadMetadatas(transformedMetadatas, cancellationToken);

        _logger.LogInformation("Metadatas loaded.");
    }
}
