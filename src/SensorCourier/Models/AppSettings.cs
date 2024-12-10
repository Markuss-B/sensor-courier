using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SensorCourier.App.Models;

public class AppSettings
{
    public double BatchDelaySeconds { get; set; }
    public int BatchSize { get; set; }

    public static readonly PropertyInfo[] PropertyInfos = typeof(AppSettings).GetProperties();
    public static readonly string[] PropertyNames = PropertyInfos.Select(p => p.Name).ToArray();

    public bool IsSet()
    {
        return PropertyInfos.All(p => p.GetValue(this) != null);
    }
}


