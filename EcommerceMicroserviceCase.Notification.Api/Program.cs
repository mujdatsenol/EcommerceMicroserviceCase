using EcommerceMicroserviceCase.Notification.Api.Features.Email;
using EcommerceMicroserviceCase.Notification.Api.Features.Email.Messaging;
using EcommerceMicroserviceCase.Notification.Api.Repositories.Extensions;
using EcommerceMicroserviceCase.Shared.Extensions;
using EcommerceMicroserviceCase.Shared.Messaging;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddDatabaseService(builder.Configuration);
builder.Services.AddRepositories();
builder.Services.AddCommonServices(typeof(Program));
builder.Services.AddRabbitMqService(builder.Configuration);
builder.Services.AddMessageConsumers();

var app = builder.Build();

app.MapEmailEndpoints();

if (app.Environment.IsDevelopment())
{
    app.ApplyMigrations();
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Notification API");
    });
}

app.Run();