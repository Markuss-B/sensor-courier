using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorCourier.Models;

[Index(nameof(TimeStamp))]
public class SensorMeasurements
{
    public int Id { get; set; }
    public DateTime TimeStamp { get; set; }
    public int SensorId { get; set; }
    public string MeasurementsJson { get; set; }
}
