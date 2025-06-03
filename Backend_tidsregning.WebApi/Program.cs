using Backend_tidsregning.Core.Context;
using Backend_tidsregning.Core.Context.ContextOptions;
using Backend_tidsregning.Core.Entities.MongoDb;
using Backend_tidsregning.Core.Interfaces.Services;
using Backend_tidsregning.Core.Services.CollectionService;
using Backend_tidsregning.Core.Services.ServiceOptions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Configuration.AddUserSecrets<Program>();
builder.Services.Configure<ContextOptions>(
    builder.Configuration.GetSection("MongoDb")
);
builder.Services.AddSingleton<IMongoClient, MongoClient>(provider =>
{
    var connectionString = builder.Configuration.GetRequiredSection("MongoDb:ConnectionString").Value;
    return new MongoClient(connectionString);
});


builder.Services.AddScoped<MongoDbContext>();

builder.Services.AddScoped<ICollectionService<Employee>, CollectionService<Employee>>(provider =>
{
    var options = Options.Create<ServiceOptions>(new()
    {
        Collection = builder.Configuration.GetRequiredSection("MongoDb:Collections:Employee").Value ?? throw new ArgumentNullException()
    });
    var context = provider.GetRequiredService<MongoDbContext>();
    return new CollectionService<Employee>(options, context);
});

builder.Services.AddScoped<ICollectionService<TimeTable>, CollectionService<TimeTable>>(provider =>
{
    var options = Options.Create<ServiceOptions>(new()
    {
        Collection = builder.Configuration.GetRequiredSection("MongoDb:Collections:TimeTable").Value ?? throw new ArgumentNullException()
    });
    var context = provider.GetRequiredService<MongoDbContext>();
    return new CollectionService<TimeTable>(options, context);
});

builder.Services.AddScoped<ICollectionService<Permission>, CollectionService<Permission>>(provider =>
{
    var options = Options.Create<ServiceOptions>(new()
    {
        Collection = builder.Configuration.GetRequiredSection("MongoDb:Collections:Permission").Value ?? throw new ArgumentNullException()
    });
    var context = provider.GetRequiredService<MongoDbContext>();
    return new CollectionService<Permission>(options, context);
});

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
