
using Buildingblocks.Exceptions.Handler;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddCarter();
var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR(conf =>
{
    conf.RegisterServicesFromAssembly(assembly);

    // register behaviour

    conf.AddOpenBehavior(typeof(ValidationBehaviour<,>));
});

// register Validation behaviour

builder.Services.AddValidatorsFromAssembly(assembly);


// Register Fluent Validation middeware 

builder.Services.AddValidatorsFromAssembly(assembly);


// register postgres context

builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("PostgresDataBase")!);
}).UseLightweightSessions();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<CustomeExceptionHandler>();

var app = builder.Build();


app.MapCarter();
app.UseExceptionHandler();
app.Run();
