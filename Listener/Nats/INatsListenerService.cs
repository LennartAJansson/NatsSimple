namespace Listener.Nats;

using System.Runtime.CompilerServices;

internal interface INatsListenerService<T>
{
  IAsyncEnumerable<T> ListenAsync(string streamName, string subjectName, string consumerName,
    CancellationToken cancellationToken = default);
}