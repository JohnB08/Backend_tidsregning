using System;
using Backend_tidsregning.Core.Entities.MongoDb;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Backend_tidsregning.Core.Context.ContextOptions;
using MongoDB.Driver;

namespace Backend_tidsregning.Core.Context;

public class MongoDbContext(IMongoClient client, IOptions<ContextOptions.ContextOptions> options)
{
    public IMongoDatabase Database { get; private set; } = client.GetDatabase(options.Value.DatabaseName);
}
