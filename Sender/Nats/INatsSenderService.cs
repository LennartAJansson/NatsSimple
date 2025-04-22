namespace Sender.Nats;

internal interface INatsSenderService
{
  Task SendMessageAsync<T>(string stream, string subject, T message, CancellationToken cancellationToken = default);
}