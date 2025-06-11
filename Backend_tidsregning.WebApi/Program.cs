using System.Text;
using Backend_tidsregning.Core.Context;
using Backend_tidsregning.Core.Context.ContextOptions;
using Backend_tidsregning.Core.Entities.MongoDb;
using Backend_tidsregning.Core.Interfaces.Services;
using Backend_tidsregning.Core.Services.CollectionService;
using Backend_tidsregning.Core.Services.ServiceOptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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

builder.Services.AddLogging(builder => builder.AddConsole());
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
        Collection = builder.Configuration.GetRequiredSection("MongoDb:Collections:Employees").Value ?? throw new ArgumentNullException()
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
        Collection = builder.Configuration.GetRequiredSection("MongoDb:Collections:Permissions").Value ?? throw new ArgumentNullException()
    });
    var context = provider.GetRequiredService<MongoDbContext>();
    return new CollectionService<Permission>(options, context);
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["SupaBase:JwtSecret"])
        ),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["SupaBase:ValidIssuer"],
        ValidateAudience = true,
        ValidAudience = "authenticated",
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});
builder.Services.AddAuthorization();

builder.Services.AddSwaggerGen(
    options =>
    {
        options.AddSecurityDefinition("BearerAuth", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Description = "Please provide your Bearer Token to use locked endpoints."
        });
        options.AddSecurityRequirement(
            new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "BearerAuth"
                        }
                    },
                    []
                }
            }
        );
    }
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
