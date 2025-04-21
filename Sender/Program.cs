using Microsoft.Extensions.Logging;

using NATS.Client.Core;
using NATS.Client.JetStream;
using NATS.Client.JetStream.Models;
using NATS.Net;

using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
ILogger<Program> logger = factory.CreateLogger<Program>();

NatsOpts opts = NatsOpts.Default with
{
  Url = "nats://nats.ubk3s:4222",
  LoggerFactory = factory
};

await using NatsClient? client = new NatsClient(opts);
INatsJSContext? jetStreamContext = client.CreateJetStreamContext();
INatsJSStream? jetStream = await jetStreamContext!.CreateStreamAsync(new StreamConfig("teststream", subjects: new string[] { "teststream.new" }));

CancellationTokenSource cts = new();

Console.CancelKeyPress += (_, e) =>
{
  e.Cancel = true;
  cts.Cancel();
};

while (!cts.IsCancellationRequested)
{
  string message = $"Hello world {DateTime.Now}";
  logger.LogInformation("Sending message: {msg}", message);
  PubAckResponse ack = await jetStreamContext!.PublishAsync("teststream.new", message);
  ack.EnsureSuccess();
  await Task.Delay(5000);
}

