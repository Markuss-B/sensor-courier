using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorCourier.Models;

[Index(nameof(Timestamp))]
[Index(nameof(SensorId))]
public class SensorMeasurement
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public string SensorId { get; set; }
    public string MetricKey { get; set; }
    public string MetricValue { get; set; }
}
