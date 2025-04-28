namespace Sender;

using System;
using System.Threading;
using System.Threading.Tasks;

using Sender.Nats;

internal sealed class Worker(ILogger<Worker> logger, INatsSenderService service)
  : BackgroundService
{
  protected override async Task ExecuteAsync(CancellationToken cancellationToken)
  {
    while (!cancellationToken.IsCancellationRequested)
    {
      string message = $"Hello world {DateTime.Now}";
      logger.LogInformation("Sending message: {msg}", message);
      await service.SendMessageAsync("teststream", "teststream.new", message, cancellationToken: cancellationToken);

      await Task.Delay(5000, cancellationToken);
    }
  }
}