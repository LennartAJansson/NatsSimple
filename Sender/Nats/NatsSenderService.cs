namespace Sender.Nats;

using System.Threading;
using System.Threading.Tasks;

using NATS.Client.JetStream;
using NATS.Client.JetStream.Models;

internal sealed class NatsSenderService
  : INatsSenderService
{
  private readonly ILogger<NatsSenderService> logger;
  private readonly INatsJSContext context;
  private INatsJSStream? stream = null;

  public NatsSenderService(ILogger<NatsSenderService> logger, INatsJSContext context)
  {
    this.logger = logger;
    this.context = context;
  }

  public async Task SendMessageAsync<T>(string streamName, string subjectName, T message, CancellationToken cancellationToken = default)
  {
    // Create a stream if it does not exist
    stream ??= await context.CreateStreamAsync(new StreamConfig(streamName, [subjectName]), cancellationToken);

    PubAckResponse ack = await context.PublishAsync(subjectName, message, cancellationToken: cancellationToken);
    ack.EnsureSuccess();
  }
}