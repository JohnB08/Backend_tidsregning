using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Backend_tidsregning.Tests.Entities.MongoDb;

public class TimeTable
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? TimeTable_ID { get; set; }  // Auto-generated ObjectId

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
