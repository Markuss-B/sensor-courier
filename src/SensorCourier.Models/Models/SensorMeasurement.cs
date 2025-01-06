using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SensorCourier.Models;

/// <summary>
/// A single MongoSensorMeasurements object is split in to multiple SensorMeasurement objects.
/// </summary>
[Index(nameof(Timestamp))]
[Index(nameof(SensorId))]
public class SensorMeasurement
{
    public int Id { get; set; }
    /// <summary>
    /// Timestamp of the measurement.
    /// </summary>
    public DateTime Timestamp { get; set; }
    /// <summary>
    /// SensorId of the measurement.
    /// </summary>
    [MaxLength(100)]
    public string SensorId { get; set; }
    /// <summary>
    /// Measurement name
    /// </summary>
    [MaxLength(100)]
    public string MetricKey { get; set; }
    /// <summary>
    /// Measurement value
    /// </summary>
    public string MetricValue { get; set; }
}
