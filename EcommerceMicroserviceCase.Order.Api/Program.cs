using EcommerceMicroserviceCase.Order.Api.Repositories;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddDatabaseService(builder.Configuration);
builder.Services.AddRepositories();

var app = builder.Build();

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
dbContext.Database.MigrateAsync().ContinueWith(x =>
{
    Console.WriteLine(x.IsFaulted
        ? $"Migration failed: {x.Exception?.Message}"
        : "Migration successful");
});

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Order API");
    });
}

app.Run();