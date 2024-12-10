using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorCourier.Models;

[Index(nameof(Timestamp))]
public class SensorMetadata
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public int SensorId { get; set; }
    public string MetadataJson { get; set; }
}
