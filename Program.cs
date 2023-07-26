var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureServices(s => s.AddSingleton(builder))
                    .ConfigureAppConfiguration(
                          ic => ic.AddJsonFile(Path.Combine("Configurations",
                                                            "configuration.json")));

var app = builder.Build();
app.Run();
