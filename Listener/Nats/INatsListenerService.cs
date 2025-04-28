namespace Listener.Nats;

internal interface INatsListenerService<T>
{
  IAsyncEnumerable<T> ListenAsync(string streamName, string subjectName, string consumerName,
    CancellationToken cancellationToken = default);
}