namespace SensorCourier.App;

public record Provider(string Name, string Assembly)
{
    public static Provider SqlServer = new(nameof(SqlServer), typeof(SqlServer.Marker).Assembly.GetName().Name!);
    public static Provider MySql = new(nameof(MySql), typeof(MySql.Marker).Assembly.GetName().Name!);
}
