using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorCourier.App.Models;

public class AppSettings
{
    public string BatchDelaySeconds { get; set; }
    public string BatchSize { get; set; }
    public DateTime LastSyncDateTime { get; set; }
    public bool Monitor { get; set; }

}
