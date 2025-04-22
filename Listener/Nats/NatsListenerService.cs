namespace Listener.Nats;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

using NATS.Client.JetStream;
using NATS.Client.JetStream.Models;

internal sealed class NatsListenerService<T>
  : INatsListenerService<T>
{
  private readonly ILogger<NatsListenerService<T>> logger;
  private readonly INatsJSContext context;
  private INatsJSStream? stream = null;
  private INatsJSConsumer? consumer = null;

  public NatsListenerService(ILogger<NatsListenerService<T>> logger, INatsJSContext context)
  {
    this.logger = logger;
    this.context = context;
  }
  public async IAsyncEnumerable<T> ListenAsync(string streamName, string subjectName, string consumerName,
    [EnumeratorCancellation] CancellationToken cancellationToken = default)
  {
    // Create a stream and consumer if they do not exist, we assume the stream already exists
    stream ??= await context.CreateStreamAsync(new StreamConfig(streamName, subjects: [subjectName]), cancellationToken);
    consumer ??= await context.CreateOrUpdateConsumerAsync(streamName, new ConsumerConfig(consumerName), cancellationToken);

    await foreach (NatsJSMsg<T> jsMsg in consumer.ConsumeAsync<T>(cancellationToken: cancellationToken))
    {
      if (jsMsg.Data is not null)
      {
        await jsMsg.AckAsync(cancellationToken: cancellationToken);
        yield return jsMsg.Data;
      }
    }
  }
}
