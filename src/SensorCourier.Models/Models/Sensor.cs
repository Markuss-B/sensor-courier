using Microsoft.EntityFrameworkCore;

namespace SensorCourier.Models;

[Index(nameof(NativeSensorId), IsUnique = true)]
public class Sensor
{
    public int Id { get; set; }
    /// <summary>
    /// As is stored in the mongodb and by the sensor itself.
    /// </summary>
    public string NativeSensorId { get; set; }
    public DateTime LastUpdated { get; set; }
    public ICollection<SensorMeasurements> SensorMeasurements { get; set; } = new List<SensorMeasurements>();
    public ICollection<SensorMetadata> SensorMetadata { get; set; } = new List<SensorMetadata>();
}
