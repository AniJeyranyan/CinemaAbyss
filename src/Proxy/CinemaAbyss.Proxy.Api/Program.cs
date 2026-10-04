using CinemaAbyss.Proxy.Api.Middleware;
using CinemaAbyss.Proxy.Application;
using CinemaAbyss.Proxy.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var port = Environment.GetEnvironmentVariable("PORT") ?? "8000";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseProxyForwarding();

app.Run();
