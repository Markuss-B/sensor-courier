using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SensorCourier.Models;

[Index(nameof(Timestamp))]
[Index(nameof(SensorId))]
public class SensorMeasurement
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    [MaxLength(100)]
    public string SensorId { get; set; }
    [MaxLength(100)]
    public string MetricKey { get; set; }
    public string MetricValue { get; set; }
}
