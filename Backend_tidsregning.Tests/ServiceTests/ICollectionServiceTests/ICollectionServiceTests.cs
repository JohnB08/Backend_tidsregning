using Backend_tidsregning.Core.Context;
using Backend_tidsregning.Core.Context.ContextOptions;
using Backend_tidsregning.Core.Entities.MongoDb;
using Backend_tidsregning.Core.Interfaces.Services;
using Backend_tidsregning.Core.Services.CollectionService;
using Backend_tidsregning.Core.Services.ServiceOptions;
using BCrypt.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Backend_tidsregning.Tests.ServiceTests.ICollectionServiceTests;


public class ICollectionServiceTests
{
    private MongoDbContext _context;
    private ICollectionService<Employee> _employeeCollection;
    public ICollectionServiceTests()
    {
        var config = new ConfigurationBuilder()
                            .AddUserSecrets<ICollectionServiceTests>()
                            .AddEnvironmentVariables()
                            .AddJsonFile("appsettings.json", false, true)
                            .Build();

        var databaseName = config.GetRequiredSection("MongoDb:DatabaseName").Value;

        var contextOptions = Options.Create<ContextOptions>(new()
        {
            DatabaseName = databaseName ?? throw new ArgumentNullException(),
        });
        _context = new(config, contextOptions);

        var collectionName = config.GetRequiredSection("MongoDb:Collections:Employees").Value;
        var collectionOptions = Options.Create<ServiceOptions>(new()
        {
            Collection = collectionName ?? throw new ArgumentNullException()
        });
        _employeeCollection = new CollectionService<Employee>(collectionOptions, _context);
    }
    [Fact]
    public async Task TestGetAllFromCollection()
    {
        var allEmployees = await _employeeCollection.GetAllAsync();
        Assert.NotNull(allEmployees);
        Assert.IsType<List<Employee>>(allEmployees);
    }
    [Fact]
    public async Task TestAddNewEmployeeToCollection()
    {
        var currentCount = await _employeeCollection.GetCountAsync();

        var success = await _employeeCollection.TryAddAsync(new Employee()
        {
            Name = "John",
            EmailHash = BCrypt.Net.BCrypt.HashPassword("Hello@World.com")
        });

        var newCount = await _employeeCollection.GetCountAsync();

        Assert.Equal(currentCount + 1, newCount);
        Assert.True(success);
    }
    [Fact]
    public async Task TestRemoveEmployeeFromCollection()
    {

        _ = await _employeeCollection.TryAddAsync(new Employee()
        {
            Name = "John",
            EmailHash = BCrypt.Net.BCrypt.HashPassword("Hello@World.com")
        });
        var currentCount = await _employeeCollection.GetCountAsync();
        var employeeList = await _employeeCollection.GetAllAsync();

        var success = await _employeeCollection.TryRemoveAsync(employeeList.Last()._ID);
        var newCount = await _employeeCollection.GetCountAsync();

        Assert.Equal(currentCount - 1, newCount);
        Assert.True(success);
    }
    [Fact]
    public async Task TestUpdateEmployeeFromCollection()
    {
        _ = await _employeeCollection.TryAddAsync(new Employee()
        {
            Name = "John",
            EmailHash = BCrypt.Net.BCrypt.HashPassword("Hello@World.com")
        });
        var employeeList = await _employeeCollection.GetAllAsync();
        var employee = employeeList.Last();
        employee.EmailHash = BCrypt.Net.BCrypt.HashPassword("Goodbye@World.com");
        var success = await _employeeCollection.TryUpdateAsync(employee);

        Assert.True(success);
    }
}