using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SensorCourier.App.Models;

public class MongoSensorMeasurements
{
    [BsonId]
    public ObjectId Id { get; set; }
    [BsonElement("timestamp")]
    public DateTime Timestamp { get; set; }
    [BsonElement("sensorId")]
    public string SensorId { get; set; }
    [BsonElement("measurements")]
    public Dictionary<string, object> Measurements { get; set; }
}
