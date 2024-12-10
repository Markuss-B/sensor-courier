﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorCourier.App.Models;

public class MongoSensorMeasurements
{
    [BsonId]
    public ObjectId Id { get; set; }
    [BsonElement("timestamp")]
    public DateTime Timestamp { get; set; }
    [BsonElement("sensorId")]
    public required string SensorId { get; set; }
    [BsonElement("measurements")]
    public required BsonDocument Measurements { get; set; }
}