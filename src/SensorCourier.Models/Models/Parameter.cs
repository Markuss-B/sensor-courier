using System.ComponentModel.DataAnnotations;

namespace SensorCourier.Models;

public class Parameter
{
    public int Id { get; set; }
    /// <summary>
    /// Name of the parameter.
    /// </summary>
    [MaxLength(100)]
    public string Name { get; set; }
    /// <summary>
    /// Value of the parameter.
    /// </summary>
    [MaxLength(255)]
    public string Value { get; set; }
}
