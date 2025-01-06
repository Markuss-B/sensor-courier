using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SensorCourier.Models;

[Index(nameof(Timestamp))]
[Index(nameof(SensorId))]
public class SensorMetadata
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    [MaxLength(100)]
    public string SensorId { get; set; }
    /// <summary>
    /// Metadata name
    /// </summary>
    [MaxLength(100)]
    public string MetaKey { get; set; }
    /// <summary>
    /// Metadata value
    /// </summary>
    public string MetaValue { get; set; }
}
