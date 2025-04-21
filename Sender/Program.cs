using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

using NATS.Client.Core;
using NATS.Client.JetStream;
using NATS.Client.JetStream.Models;

NatsConnection? connection;
INatsJSContext? jetStreamContext;
using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());

NatsOpts opts = NatsOpts.Default with
{
  Url = "nats://nats.ubk3s:4222",
  LoggerFactory = factory
};

connection = new NatsConnection(opts);
jetStreamContext = new NatsJSContext(connection);

_ = await jetStreamContext!.CreateStreamAsync(new StreamConfig("teststream", subjects: new string[] { "teststream.new" }));


CancellationTokenSource cts = new();

Console.CancelKeyPress += (_, e) =>
{
  e.Cancel = true;
  cts.Cancel();
};

while (!cts.IsCancellationRequested)
{
  // Publish a message to the stream
  string message = $"Hello world {DateTime.Now}";
  PubAckResponse ack = await jetStreamContext!.PublishAsync(subject: "teststream.new", data: message);
  ack.EnsureSuccess();
  await Task.Delay(5000);
}

