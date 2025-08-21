using CatalogoApi.Context;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;

Env.Load();

Console.WriteLine("DB_SERVER: " + Environment.GetEnvironmentVariable("DB_SERVER"));
Console.WriteLine("DB_DATABASE: " + Environment.GetEnvironmentVariable("DB_DATABASE"));
Console.WriteLine("DB_USER: " + Environment.GetEnvironmentVariable("DB_USER"));
Console.WriteLine("DB_PASSWORD: " + Environment.GetEnvironmentVariable("DB_PASSWORD"));

var builder = WebApplication.CreateBuilder(args);

// Add services to the container. --> ConfigureServices()
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string? mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => 
                options.UseMySql(mySqlConnection, 
                ServerVersion.AutoDetect(mySqlConnection)));

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
