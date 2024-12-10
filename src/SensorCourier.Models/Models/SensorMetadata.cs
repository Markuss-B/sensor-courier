using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorCourier.Models;

[Index(nameof(Timestamp))]
[Index(nameof(SensorId))]
public class SensorMetadata
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    [MaxLength(100)]
    public string SensorId { get; set; }
    [MaxLength(100)]
    public string MetaKey { get; set; }
    public string MetaValue { get; set; }
}
