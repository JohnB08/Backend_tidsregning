# Current plan for POCO entities in prosject:
![Excalidraw sketch of images](./POCO%20Entities.excalidraw.png)

Our current plan for the POCO Entities is the following code:

```cs
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Employee
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Employee_ID { get; set; }  // GUID primary key

    [BsonElement("Email_Hash")]
    public string EmailHash { get; set; }

    [BsonElement("Navn")]
    public string Name { get; set; }

    [BsonElement("TotalPermissionDays")]
    [BsonDefaultValue(31)]
    [BsonIgnoreIfDefault]
    public int TotalPermissionDays { get; set; } = 31;
}

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

public class Permission
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Permission_ID { get; set; }  // Auto-generated ObjectId

    [BsonElement("Employee_ID")]
    [BsonRepresentation(BsonType.String)]
    public Guid EmployeeId { get; set; }  // References Employee.Employee_ID

    [BsonElement("Date")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime Date { get; set; }

    [BsonElement("Description")]
    public string Description { get; set; }
}

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

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
```

