using NATS.Client.Hosting;

using Sender;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.AddNats((opts) =>
{
  return opts with
  {
    Url = "nats://nats.ubk3s:4222",
    LoggerFactory = builder.Services
        .BuildServiceProvider()
        .GetRequiredService<ILoggerFactory>()
  };
});

builder.Services.AddHostedService<Worker>();

IHost host = builder.Build();

await host.RunAsync();
