using Ocelot.Configuration.File;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Eureka;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile(Path.Combine("Configurations", "configuration.json"));
builder.Services.AddOcelot().AddEureka();
var app = builder.Build();
app.UseOcelot().Wait();
app.UseAppSwagger(builder.Configuration);
app.Run();



public static class AppSwagger
{
    public static void UseAppSwagger(this IApplicationBuilder app, IConfiguration configuration)
    {
        app.UseSwaggerUI(setup =>
        {
            setup.RoutePrefix = "docs";
            var _httpClient = new HttpClient();
            foreach (var service in configuration.Get<FileConfiguration>().Routes)
            {
                var address = service.DownstreamHostAndPorts.First();
                var url = $"{service.DownstreamScheme}://{address.Host}:{address.Port}/swagger/v1/swagger.json";
                var jsonEndpoint = $"/swagger/v1/swagger.json";

                app.Map(jsonEndpoint, b =>
                {
                    b.Run(async x =>
                    {
                        string content = await _httpClient.GetStringAsync(url);
                        Console.WriteLine("My swagger url", url);
                        Console.WriteLine("My swagger content", content);
                        await x.Response.WriteAsync(content);
                    });
                });

                setup.SwaggerEndpoint(jsonEndpoint, service.Key);
            }
        });
    }
}