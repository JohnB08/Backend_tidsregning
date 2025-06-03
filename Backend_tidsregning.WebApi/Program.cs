using Backend_tidsregning.Core.Context;
using Backend_tidsregning.Core.Context.ContextOptions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Configuration.AddUserSecrets<Program>();
builder.Services.Configure<ContextOptions>(
    builder.Configuration.GetSection("MongoDb")
);
builder.Services.AddScoped<MongoDbContext>();
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
