﻿using System.ComponentModel.DataAnnotations;

namespace SensorCourier.Models;

public class Parameter
{
    public int Id { get; set; }
    [MaxLength(100)]
    public string Name { get; set; }
    [MaxLength(255)]
    public string Value { get; set; }
}
