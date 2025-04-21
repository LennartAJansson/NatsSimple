namespace Sender;

using System;
using System.Threading;
using System.Threading.Tasks;

using NATS.Client.JetStream;
using NATS.Client.JetStream.Models;

internal sealed class Worker(ILogger<Worker> logger, INatsJSContext context)
  : BackgroundService
{
  protected override async Task ExecuteAsync(CancellationToken cancellationToken)
  {
    // Create a stream if it does not exist
    _ = await context.CreateStreamAsync(new StreamConfig("teststream", ["teststream.new"]), cancellationToken);

    while (!cancellationToken.IsCancellationRequested)
    {
      string message = $"Hello world {DateTime.Now}";
      logger.LogInformation("Sending message: {msg}", message);
      PubAckResponse ack = await context.PublishAsync("teststream.new", message, cancellationToken: cancellationToken);
      ack.EnsureSuccess();

      await Task.Delay(5000, cancellationToken);
    }
  }
}
