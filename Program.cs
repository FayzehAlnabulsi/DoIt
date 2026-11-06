
using DoIt.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

Console.WriteLine($"Connection string exists: {!string.IsNullOrWhiteSpace(connectionString)}");

if (!string.IsNullOrWhiteSpace(connectionString))
{
    var connectionBuilder = new MySql.Data.MySqlClient.MySqlConnectionStringBuilder(connectionString);

    Console.WriteLine($"MySQL Host: [{connectionBuilder.Server}]");
    Console.WriteLine($"MySQL Port: [{connectionBuilder.Port}]");
    Console.WriteLine($"MySQL Database: [{connectionBuilder.Database}]");
}

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySQL(connectionString)
);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();
