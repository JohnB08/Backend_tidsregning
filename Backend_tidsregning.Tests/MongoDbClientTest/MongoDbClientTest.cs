using System;
using Backend_tidsregning.Core.Context;
using Backend_tidsregning.Core.Context.ContextOptions;
using Backend_tidsregning.Core.Entities.MongoDb;
using Backend_tidsregning.Core.Services.ServiceOptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Backend_tidsregning.Tests.MongoDbClientTest;

public class MongoDbClientTest
{
    private MongoClient _client;
    private IConfiguration _config;
    public MongoDbClientTest()
    {

        _config = new ConfigurationBuilder()
                    .AddUserSecrets<MongoDbClientTest>()
                    .AddEnvironmentVariables()
                    .AddJsonFile("appsettings.json")
                    .Build();
        var connectionString = _config["MongoDb:ConnectionString"];
        var settings = MongoClientSettings.FromConnectionString(connectionString);
        settings.ServerApi = new ServerApi(ServerApiVersion.V1);
        _client = new MongoClient(settings);
    }
    [Fact]
    public async Task TestFetchDocumentFromDb()
    {
        var mongoDbConfig = _config.GetRequiredSection("MongoDb");
        var contextOptions = Options.Create<ContextOptions>( new ContextOptions()
        {
            DatabaseName = mongoDbConfig.GetRequiredSection("DatabaseName").Value ?? throw new ArgumentNullException(),
        });
        var collectionOptions = Options.Create<ServiceOptions>(new ServiceOptions()
        {
            Collection = mongoDbConfig.GetRequiredSection("Collections:Employees").Value ?? throw new ArgumentNullException()
        });
        //Act 
        var mongoClient = new MongoDbContext(_client, contextOptions);
        var employeeCollection = mongoClient.Database.GetCollection<Employee>(collectionOptions.Value.Collection);
        var filter = Builders<Employee>.Filter.Empty;
        var result = await employeeCollection.FindAsync(filter);
        var employeeList = await result.ToListAsync();
        //Assert
        Assert.NotNull(employeeList);
        Assert.IsType<List<Employee>>(employeeList);

    }
}
