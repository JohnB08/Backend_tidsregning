using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Backend_tidsregning.Core.Entities.MongoDb;

public class Permission
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public Guid _ID { get; set; }  // Auto-generated ObjectId

    [BsonElement("Employee_ID")]
    [BsonRepresentation(BsonType.String)]
    public Guid EmployeeId { get; set; }  // References Employee.Employee_ID

    [BsonElement("Date")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime Date { get; set; }

    [BsonElement("Description")]
    public required string Description { get; set; }
}