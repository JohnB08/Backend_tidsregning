using System;
using MongoDB.Driver;

namespace Backend_tidsregning.Core.Context;

public class MongoDbContext
{
    public MongoClient Client { get; private set;}
    public MongoDbContext(string connectionString)
    {
        var settings = MongoClientSettings.FromConnectionString(connectionString);
        settings.ServerApi = new ServerApi(ServerApiVersion.V1);
        Client = new MongoClient(settings);
    }
}
