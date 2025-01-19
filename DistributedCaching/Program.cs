using DistributedCaching;
using DistributedCaching.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<IProductService, ProductServiceDistributedCaching>();
builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultDatabaseConnection"));
});
builder.Services.AddSwaggerGen();

// in memory caching
builder.Services.AddMemoryCache();

// distributed caching configuration
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost";
    options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions()
    {
        AbortOnConnectFail = true, // application fails immediately in case it cannot connect to Redis
        EndPoints = { options.Configuration } // ensuring that the endpoint for the Redis server is correctly set __ options.Configuration
    };
});

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
