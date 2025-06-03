using System;
using Backend_tidsregning.Core.Interfaces.Entites;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Backend_tidsregning.Core.Entities.MongoDb;

public class Employee : IMongoDbId
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid _ID { get; set; }  // GUID primary key

    [BsonElement("Email_Hash")]
    public required string EmailHash { get; set; }

    [BsonElement("Navn")]
    public required string Name { get; set; }

    [BsonElement("TotalPermissionDays")]
    [BsonDefaultValue(31)]
    [BsonIgnoreIfDefault]
    public int TotalPermissionDays { get; set; } = 31;
}
