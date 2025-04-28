namespace Listener;

using System.Threading;
using System.Threading.Tasks;

using Listener.Nats;

internal sealed class Worker(ILogger<Worker> logger, INatsListenerService<string> service)
  : BackgroundService
{
  protected override async Task ExecuteAsync(CancellationToken cancellationToken)
  {
    while (!cancellationToken.IsCancellationRequested)
    {
      await foreach (string message in service.ListenAsync("teststream", "teststream.new", "testconsumer", cancellationToken: cancellationToken))
      {
        if (message is not null)
        {
          logger.LogInformation("Received message: {msg}", message);
        }
      }
    }
  }
}