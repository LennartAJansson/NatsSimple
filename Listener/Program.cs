using Microsoft.Extensions.Logging;

using NATS.Client.Core;
using NATS.Client.JetStream;
using NATS.Client.JetStream.Models;

NatsConnection? connection;
INatsJSContext? jetStreamContext;
INatsJSStream? jetStream;
INatsJSConsumer? consumer;

using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());

NatsOpts opts = NatsOpts.Default with
{
  Url = $"nats://nats.ubk3s:4222",
  LoggerFactory = factory
};

connection = new NatsConnection(opts);
jetStreamContext = new NatsJSContext(connection);
jetStream = await jetStreamContext!.CreateStreamAsync(new StreamConfig("teststream", subjects: new string[] { "teststream.new" }));
consumer = await jetStreamContext.CreateOrUpdateConsumerAsync("teststream", new ConsumerConfig("testconsumer2"));

CancellationTokenSource cts = new();

Console.CancelKeyPress += (_, e) =>
{
  e.Cancel = true;
  cts.Cancel();
};

//Read all items from the channel.
await foreach (NatsJSMsg<string> jsMsg in consumer!.ConsumeAsync<string>(cancellationToken: cts.Token))
{
  if (jsMsg.Data is not null)
  {
    Console.WriteLine($"Received message: {jsMsg.Data}");
    await jsMsg.AckAsync();
  }
}