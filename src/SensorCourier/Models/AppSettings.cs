using System.Reflection;

namespace SensorCourier.App.Models;

public class AppSettings
{
    public double BatchDelaySeconds { get; set; }
    public int BatchSize { get; set; }

    public static readonly PropertyInfo[] PropertyInfos = typeof(AppSettings).GetProperties();
    public static readonly string[] PropertyNames = PropertyInfos.Select(p => p.Name).ToArray();

    /// <summary>
    /// Check if all properties are set.
    /// </summary>
    public bool IsSet()
    {
        return PropertyInfos.All(p => p.GetValue(this) != null);
    }
}


