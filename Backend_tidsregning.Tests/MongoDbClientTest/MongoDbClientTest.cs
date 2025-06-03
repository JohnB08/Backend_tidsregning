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
    [Fact]
    public void TestFetchConnectionString()
    {
        //Arrange
        var config = new ConfigurationBuilder().AddUserSecrets<MongoDbClientTest>().AddEnvironmentVariables().Build();
        //Act
        var connectionString = config["MongoDb:ConnectionString"];
        if (string.IsNullOrWhiteSpace(connectionString)) connectionString = config["MONGO_DB_CONNECTION_STRING"];
        //Assert
        Assert.NotNull(connectionString);
        Assert.True(!string.IsNullOrWhiteSpace(connectionString));
    }
    [Fact]
    public async Task TestFetchDocumentFromDb()
    {
        //Arrange
        var config = new ConfigurationBuilder()
                            .AddUserSecrets<MongoDbClientTest>()
                            .AddEnvironmentVariables()
                            .AddJsonFile("appsettings.json", false, true)
                            .Build();
        var mongoDbConfig = config.GetRequiredSection("MongoDb");
        var contextOptions = Options.Create<ContextOptions>( new ContextOptions()
        {
            DatabaseName = mongoDbConfig.GetRequiredSection("DatabaseName").Value ?? throw new ArgumentNullException(),
        });
        var collectionOptions = Options.Create<ServiceOptions>(new ServiceOptions()
        {
            Collection = mongoDbConfig.GetRequiredSection("Collections:Employees").Value ?? throw new ArgumentNullException()
        });
#pragma warning restore CS8601 // Possible null reference assignment.
        //Act 
        var mongoClient = new MongoDbContext(config!, contextOptions);
        var employeeCollection = mongoClient.Database.GetCollection<Employee>(collectionOptions.Value.Collection);
        var filter = Builders<Employee>.Filter.Empty;
        var result = await employeeCollection.FindAsync(filter);
        var employeeList = await result.ToListAsync();
        //Assert
        Assert.NotNull(employeeList);
        Assert.IsType<List<Employee>>(employeeList);

    }
}
