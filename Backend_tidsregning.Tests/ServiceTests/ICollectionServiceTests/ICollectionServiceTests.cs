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

    /* Her har vi laget en Employees Theorydatacollection nedenfor som inneholder en enkel liste over employee data. 
    Theory attributten sier at testen skal kjøre for alle parameterene vi har.
    Legg merke til emp parametere i testen. Vi kan se for oss at denne testen kjøres for each employee in employees.
    Det lar oss teste flere potensielle usecases / edgecases av employers. */
    [Theory]
    [MemberData(nameof(Employees), MemberType = typeof(ICollectionServiceTests))]
    public async Task TestRemoveEmployeeFromCollection(Employee emp)
    {

        _ = await _employeeCollection.TryAddAsync(emp);
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

    public static TheoryData<Employee> Employees => new()
    {
      new Employee()
        {
            Name = "John",
            EmailHash = BCrypt.Net.BCrypt.HashPassword("Hello@World.com")
        },
        new Employee()
        {
            Name = "David",
            EmailHash = BCrypt.Net.BCrypt.HashPassword("OldTown@World.com")
        },
        new Employee()
        {
            Name = "Oscar",
            EmailHash = BCrypt.Net.BCrypt.HashPassword("Mad@World.com")
        },
        new Employee()
        {
            Name = "Irene",
            EmailHash = BCrypt.Net.BCrypt.HashPassword("New@World.com")
        },
        new Employee()
        {
            Name = "Andrea",
            EmailHash = BCrypt.Net.BCrypt.HashPassword("Missing@World.com")
        }
    };
}
