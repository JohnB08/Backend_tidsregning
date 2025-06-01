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
        var config = new ConfigurationBuilder().AddUserSecrets<MongoDbClientTest>().AddEnvironmentVariables().Build();
        //Act
        var connectionString = config["MongoDb:ConnectionString"] ?? config["MONGO_DB_CONNECTION_STRING"];
        //Assert
        Assert.NotNull(connectionString);
        Assert.True(!string.IsNullOrWhiteSpace(connectionString));
    }
    [Fact]
    public async Task TestFetchDocumentFromDb()
    {
        //Arrange
        var config = new ConfigurationBuilder().AddUserSecrets<MongoDbClientTest>().AddEnvironmentVariables().Build();
        //Act 
        var mongoClient = new MongoDbContext(config!);
        var filter = Builders<Employee>.Filter.Empty;
        var result = await mongoClient.Employees.FindAsync(Builders<Employee>.Filter.Empty);
        var employeeList = await result.ToListAsync();
        //Assert
        Assert.NotNull(employeeList);
        Assert.IsType<List<Employee>>(employeeList);

    }
}
