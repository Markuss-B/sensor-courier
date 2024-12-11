using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorCourier.App.Models;

public class MongoSensorMetadatas
{
    [BsonId]
    public ObjectId Id { get; set; }
    [BsonElement("sensorId")]
    public string SensorId { get; set; }
    [BsonElement("timestamp")]
    public DateTime Timestamp { get; set; }
    [BsonElement("metadata")]
    public Dictionary<string, object> Metadata { get; set; }
}
