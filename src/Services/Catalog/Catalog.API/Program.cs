var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCarter();

builder.Services.AddMediatR(conf =>
{
    conf.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

// register postgres context

builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("PostgresDataBase")!);
}).UseLightweightSessions();

var app = builder.Build();


app.MapCarter();
app.Run();
