using System;
using Backend_tidsregning.Core.Interfaces.Entites;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Backend_tidsregning.Core.Entities.MongoDb;

public class TimeTable : IMongoDbId
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public Guid _ID { get; set; }  // Auto-generated ObjectId

    [BsonElement("Employee_ID")]
    [BsonRepresentation(BsonType.String)]
    public Guid EmployeeId { get; set; }  // References Employee.Employee_ID

    [BsonElement("Check_In")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime CheckIn { get; set; } = DateTime.UtcNow;

    [BsonElement("Check_Out")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime CheckOut { get; set; } = DateTime.UtcNow;
}
