namespace Listener;

using System.Threading;
using System.Threading.Tasks;

using NATS.Client.JetStream;
using NATS.Client.JetStream.Models;

internal sealed class Worker(ILogger<Worker> logger, INatsJSContext context)
  : BackgroundService
{
  protected override async Task ExecuteAsync(CancellationToken cancellationToken)
  {
    // Create a stream and consumer if they do not exist, we assume the stream already exists
    //INatsJSStream jetStream = await context.CreateStreamAsync(new StreamConfig("teststream", subjects: new string[] { "teststream.new" }), cancellationToken);
    INatsJSConsumer consumer = await context.CreateOrUpdateConsumerAsync("teststream", new ConsumerConfig("testconsumer"), cancellationToken);

    while (!cancellationToken.IsCancellationRequested)
    {
      await foreach (NatsJSMsg<string> jsMsg in consumer.ConsumeAsync<string>(cancellationToken: cancellationToken))
      {
        if (jsMsg.Data is not null)
        {
          logger.LogInformation("Received message: {msg}", jsMsg.Data);
          await jsMsg.AckAsync(cancellationToken: cancellationToken);
        }
      }

      await Task.Delay(5000, cancellationToken);
    }
  }
}
