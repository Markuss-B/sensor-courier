using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorCourier.App.Models;

public class MongoSensor
{
    [BsonId]
    public string Id { get; set; }
    [BsonElement("lastUpdated")]
    public DateTime LastUpdated { get; set; }
    [BsonExtraElements]
    public BsonDocument Metadata { get; set; }
}
