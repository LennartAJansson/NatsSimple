using Microsoft.Extensions.Logging;

using NATS.Client.Core;
using NATS.Client.JetStream;
using NATS.Client.JetStream.Models;
using NATS.Net;

using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
ILogger<Program> logger = factory.CreateLogger<Program>();

NatsOpts opts = NatsOpts.Default with
{
  Url = $"nats://nats.ubk3s:4222",
  LoggerFactory = factory
};

await using NatsClient client = new NatsClient(opts);
INatsJSContext? jetStreamContext = client.CreateJetStreamContext();
INatsJSStream? jetStream = await jetStreamContext!.CreateStreamAsync(new StreamConfig("teststream", subjects: new string[] { "teststream.new" }));
INatsJSConsumer? consumer = await jetStreamContext.CreateOrUpdateConsumerAsync("teststream", new ConsumerConfig("testconsumer2"));

CancellationTokenSource cts = new();

Console.CancelKeyPress += (_, e) =>
{
  e.Cancel = true;
  cts.Cancel();
};

await foreach (NatsJSMsg<string> jsMsg in consumer!.ConsumeAsync<string>()
  .WithCancellation(cts.Token))
{
  if (jsMsg.Data is not null)
  {
    logger.LogInformation("Received message: {msg}", jsMsg.Data);
    await jsMsg.AckAsync(cancellationToken: cts.Token);
  }
}