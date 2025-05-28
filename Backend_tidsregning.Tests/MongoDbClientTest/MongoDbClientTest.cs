using System;
using Backend_tidsregning.Core.Context;
using Backend_tidsregning.Core.Entities.MongoDb;
using Microsoft.Extensions.Configuration;

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
    public void TestPingToDb()
    {
        //Arrange
        var config = new ConfigurationBuilder().AddUserSecrets<MongoDbClientTest>().Build();
        var connectionString = config["MongoDb:ConnectionString"];

        //Act 
        var mongoClient = new MongoDbContext(connectionString!);
        var ping = mongoClient.Client.GetDatabase("Tidsstyring").GetCollection<Employee>("Employees");
        //Assert
        Assert.NotNull(ping);
    }
}
