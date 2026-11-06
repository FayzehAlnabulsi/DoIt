
using DoIt.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

Console.WriteLine($"Connection string exists: {!string.IsNullOrWhiteSpace(connectionString)}");

if (!string.IsNullOrWhiteSpace(connectionString))
{
    var connectionBuilder =
        new MySql.Data.MySqlClient.MySqlConnectionStringBuilder(connectionString);

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

app.MapGet("/diagnostics/db-network", async () =>
{
    const string host = "mysql-28aaefd0-fayzehalnabulsi-b55e.f.aivencloud.com";
    const int port = 22249;

    try
    {
        var addresses = await System.Net.Dns.GetHostAddressesAsync(host);

        using var client = new System.Net.Sockets.TcpClient();

        await client.ConnectAsync(host, port);

        return Results.Ok(new
        {
            DnsResolved = true,
            Addresses = addresses.Select(x => x.ToString()),
            TcpConnected = client.Connected
        });
    }
    catch (Exception ex)
    {
        return Results.Problem(
            title: ex.GetType().Name,
            detail: ex.Message
        );
    }
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();
