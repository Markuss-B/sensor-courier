using Microsoft.EntityFrameworkCore;
using SensorCourier.App.Data;
using SensorCourier.App.Models;
using SensorCourier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorCourier.App.Services;

public class SensorService
{
    private readonly ILogger<SensorService> _logger;
    private readonly ISensorRepository _sensorRepository;
    private readonly IDbContextFactory<TargetDbContext> _targetDbContextFactory;

    public SensorService(ILogger<SensorService> logger, ISensorRepository sensorRepository, IDbContextFactory<TargetDbContext> dbContextFactory)
    {
        _logger = logger;
        _sensorRepository = sensorRepository;
        _targetDbContextFactory = dbContextFactory;
    }

    public async Task<IEnumerable<MongoSensor>> GetMongoSensorsAsync(DateTime lastDateTime)
    {
        return await _sensorRepository.GetSensorsAsync(lastDateTime);
    }

    public async Task<IEnumerable<MongoSensorMeasurements>> GetMongoSensorMeasurementsAsync(DateTime lastDateTime, int batchSize)
    {
        return await _sensorRepository.GetSensorMeasurementsAsync(lastDateTime, batchSize);
    }

    public async Task SaveSensorsAsync(IEnumerable<MongoSensor> mongoSensors)
    {
        using TargetDbContext db = _targetDbContextFactory.CreateDbContext();

        var sensorIds = mongoSensors.Select(ms => ms.Id).ToList();
        var sensors = db.Sensors.Where(s => sensorIds.Contains(s.NativeSensorId));



        foreach (var mongoSensor in mongoSensors)
        {
            // Your logic here
        }
    }

    public async Task SaveSensorMeasurementsAsync(IEnumerable<MongoSensorMeasurements> sensorMeasurements)
    {
        using TargetDbContext targetDbContext = _targetDbContextFactory.CreateDbContext();
        await targetDbContext.SensorMeasurements.AddRangeAsync(sensorMeasurements);
        await targetDbContext.SaveChangesAsync();
    }
}
