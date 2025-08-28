using System.Text.Json.Serialization;
using CatalogoApi.Context;
using CatalogoApi.Services;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using Microsoft.AspNetCore.Mvc;

Env.Load();

var dbServer = Environment.GetEnvironmentVariable("DB_SERVER");
var dbDatabase = Environment.GetEnvironmentVariable("DB_DATABASE");
var dbUser = Environment.GetEnvironmentVariable("DB_USER");
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container. --> ConfigureServices()
builder.Services.AddControllers().AddJsonOptions(options =>
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string? mySqlConnection = $"Server={dbServer};DataBase={dbDatabase};Uid={dbUser};Pwd={dbPassword}";

builder.Services.AddDbContext<AppDbContext>(options => 
                options.UseMySql(mySqlConnection, 
                ServerVersion.AutoDetect(mySqlConnection)));

builder.Services.AddTransient<IMeuServico, MeuServico>();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.DisableImplicitFromServicesParameters = true;
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
