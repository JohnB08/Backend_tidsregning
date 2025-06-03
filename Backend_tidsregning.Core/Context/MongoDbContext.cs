using System;
using Backend_tidsregning.Core.Entities.MongoDb;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Backend_tidsregning.Core.Context.ContextOptions;
using MongoDB.Driver;

namespace Backend_tidsregning.Core.Context;

public class MongoDbContext
{
    private MongoClient _client;
    public IMongoDatabase Database { get; private set; }
    public MongoDbContext(IConfiguration config, IOptions<ContextOptions.ContextOptions> options)
    {
        var connectionString = config["MongoDb:ConnectionString"];
        if (string.IsNullOrWhiteSpace(connectionString)) connectionString = config["MONGO_DB_CONNECTION_STRING"];
        var settings = MongoClientSettings.FromConnectionString(connectionString);
        settings.ServerApi = new ServerApi(ServerApiVersion.V1);
        _client = new MongoClient(settings);
        Database = _client.GetDatabase(options.Value.DatabaseName);
    }
}
