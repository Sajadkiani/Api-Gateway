using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile(Path.Combine("Configurations", "configuration.json"));
builder.Services.AddOcelot();
var app = builder.Build();
app.UseOcelot().Wait();
app.Run();
