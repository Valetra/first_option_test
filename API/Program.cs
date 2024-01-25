using Microsoft.EntityFrameworkCore;

using Services;
using DAL.Contexts;
using DAL.Repositories;
using DAL.Models;
using Mapper;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

string? databaseConnectionString = configuration.GetConnectionString("database");

if (databaseConnectionString is null)
{
    string message = "Connection string \"database\" is required";
    Console.Error.WriteLine(message);
    return;
}

builder.Services.AddDbContext<SupplyContext>(options => options.UseNpgsql(databaseConnectionString));
builder.Services.AddDbContext<OrderContext>(options => options.UseNpgsql(databaseConnectionString));

builder.Services.AddScoped<IBaseRepository<Supply, Guid>, BaseRepository<Supply, Guid>>(serviceProvider =>
    new BaseRepository<Supply, Guid>(serviceProvider.GetRequiredService<SupplyContext>()));
builder.Services.AddScoped<IBaseRepository<Order, Guid>, BaseRepository<Order, Guid>>(serviceProvider =>
    new BaseRepository<Order, Guid>(serviceProvider.GetRequiredService<OrderContext>()));

builder.Services.AddScoped<ISupplyService, SupplyService>();
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddAutoMapper(typeof(AppMappingProfile));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.ToString());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
