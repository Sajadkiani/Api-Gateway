using MMLib.SwaggerForOcelot.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Eureka;
using Ocelot.Provider.Polly;
using Steeltoe.Discovery.Client;
using Steeltoe.Discovery.Eureka;

var builder = WebApplication.CreateBuilder(args);

string routes = "";
#if DEBUG
routes = Path.Combine("Configurations", "Routes.dev");
#else
routes = Path.Combine("Configurations", "Routes.prod");
#endif

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: false)
                        .AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", optional: true);

builder.Configuration.AddOcelotWithSwaggerSupport(options =>
{
    options.Folder = routes;
    options.FileOfSwaggerEndPoints = "ocelot.SwaggerEndPoints";
});

//Ocelot configs
builder.Services.AddOcelot(builder.Configuration).AddEureka();
builder.Services.AddSwaggerForOcelot(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddServiceDiscovery(o => o.UseEureka());
// builder.Services.AddDiscoveryClient();
 builder.Services.AddControllers();


var app = builder.Build();
if(app.Environment.IsDevelopment())
{
     app.UseSwagger();
}
app.UseSwaggerForOcelotUI(options => {
    options.PathToSwaggerGenerator = "/swagger/docs";
}).UseOcelot().Wait();

app.MapControllers();
app.Run();
