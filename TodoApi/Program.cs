using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using TodoApi.MicrosoftEntraId;
using TodoApi.MicrosoftGraph;
using TodoApi.Models;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("TodoContext");

if (connectionString is null)
{
    throw new InvalidOperationException("Connection string 'TodoContext' not found.");
}

var settings = Settings.LoadSettings();

// Initialize Graph
InitializeGraph(settings);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<TodoContext>(opt =>
    opt.UseInMemoryDatabase("TodoList"));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Display access token
await DisplayAccessTokenAsync();

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


void InitializeGraph(Settings settings)
{
    GraphHelper.InitializeGraphForAppOnlyAuth(settings);
}

async Task DisplayAccessTokenAsync()
{
    try
    {
        var appOnlyToken = await GraphHelper.GetAppOnlyTokenAsync();
        Console.WriteLine($"App-only token: {appOnlyToken}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error getting app-only access token: {ex.Message}");
    }
}
