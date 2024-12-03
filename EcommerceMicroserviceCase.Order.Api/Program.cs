using EcommerceMicroserviceCase.Order.Api.Features.Orders;
using EcommerceMicroserviceCase.Order.Api.Features.Outbox;
using EcommerceMicroserviceCase.Order.Api.Repositories.Extensions;
using EcommerceMicroserviceCase.Shared.Extensions;
using EcommerceMicroserviceCase.Shared.Logger;
using EcommerceMicroserviceCase.Shared.Messaging;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddDatabaseService(builder.Configuration);
builder.Services.AddRepositories();
builder.Services.AddCommonServices(typeof(Program));
builder.Services.AddRabbitMqService(builder.Configuration);
builder.Services.AddOutboxMessaging(); // Mesajlar Outbox yöntem ile gönderiliyor.
builder.Services.AddLogger(builder.Configuration);
builder.Services.ConfigureCors();

var app = builder.Build();

app.MapOrderEndpoints();

if (app.Environment.IsDevelopment())
{
    app.ApplyMigrations();
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Order API");
    });
    app.UseCors();
}

app.Run();