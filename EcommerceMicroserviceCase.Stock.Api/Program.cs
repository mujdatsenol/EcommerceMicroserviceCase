using EcommerceMicroserviceCase.StockService.Api.Repositories;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddDatabaseService(builder.Configuration);
builder.Services.AddRepositories();

var app = builder.Build();

app.AddSeedData().ContinueWith(x =>
{
    Console.WriteLine(x.IsFaulted
        ? $"Seeding failed: {x.Exception?.Message}"
        : "Seeding successful");
});

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Stock API");
    });
}

app.Run();