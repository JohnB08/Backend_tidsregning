using System;
using Backend_tidsregning.Core.Context;
using Backend_tidsregning.Core.Entities.MongoDb;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Backend_tidsregning.Tests.MongoDbClientTest;

public class MongoDbClientTest
{
    [Fact]
    public void TestFetchConnectionString()
    {
        //Arrange
        var config = new ConfigurationBuilder().AddUserSecrets<MongoDbClientTest>().Build();
        //Act
        var connectionString = config["MongoDb:ConnectionString"];
        //Assert
        Assert.NotNull(connectionString);
        Assert.True(!string.IsNullOrWhiteSpace(connectionString));
    }
    [Fact]
    public async Task TestPingToDb()
    {
        //Arrange
        var config = new ConfigurationBuilder().AddUserSecrets<MongoDbClientTest>().Build();
        //Act 
        var mongoClient = new MongoDbContext(config!);
        var filter = Builders<Employee>.Filter.Empty;
        var count = await mongoClient.Employees.CountDocumentsAsync(filter);
        //Assert
        Assert.Equal(0, count);
    }
}
