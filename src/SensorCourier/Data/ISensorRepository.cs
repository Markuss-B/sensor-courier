using SensorCourier.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorCourier.App.Data;

public interface ISensorRepository
{
    public Task<IEnumerable<MongoSensor>> GetSensorsAsync(DateTime lastDateTime);
    public Task<IEnumerable<MongoSensorMeasurements>> GetSensorMeasurementsAsync(DateTime lastDateTime, int batchSize);
}
