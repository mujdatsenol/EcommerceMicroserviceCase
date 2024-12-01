using EcommerceMicroserviceCase.Shared.Extensions;
using EcommerceMicroserviceCase.Shared.Messaging;
using EcommerceMicroserviceCase.Stock.Api.Features.Product;
using EcommerceMicroserviceCase.Stock.Api.Features.Product.Messaging;
using EcommerceMicroserviceCase.Stock.Api.Repositories.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddDatabaseService(builder.Configuration);
builder.Services.AddRepositories();
builder.Services.AddCommonServices(typeof(Program));
builder.Services.AddRabbitMqService(builder.Configuration);
builder.Services.AddMessageConsumers();

var app = builder.Build();

app.MapProductEndpoints();

if (app.Environment.IsDevelopment())
{
    app.ApplyMigrations();
    app.AddSeedData().ContinueWith(x =>
    {
        Console.WriteLine(x.IsFaulted
            ? $"Seeding failed: {x.Exception?.Message}"
            : "Seeding successful");
    });
    
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Stock API");
    });
}

app.Run();