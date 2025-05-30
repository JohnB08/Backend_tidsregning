using System;
using Backend_tidsregning.Core.Entities.MongoDb;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Backend_tidsregning.Core.Context;

public class MongoDbContext
{
    public MongoClient Client { get; private set;}

    public IMongoCollection<Employee> Employees { get; private set; }

    public IMongoCollection<TimeTable> TimeTables { get; private set; }

    public IMongoCollection<Permission> Permissions { get; private set; }
    public MongoDbContext(IConfiguration config)
    {
        var settings = MongoClientSettings.FromConnectionString(config["MongoDb:ConnectionString"]);
        settings.ServerApi = new ServerApi(ServerApiVersion.V1);
        Client = new MongoClient(settings);
        Employees = Client.GetDatabase("Tidsstyring").GetCollection<Employee>("Employees");
        TimeTables = Client.GetDatabase("Tidsstyring").GetCollection<TimeTable>("TimeTables");
        Permissions = Client.GetDatabase("Tidsstyring").GetCollection<Permission>("Permissions");
    }
}
