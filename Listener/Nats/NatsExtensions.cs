namespace Listener.Nats;

using Listener.Nats;

using NATS.Client.Core;
using NATS.Client.Hosting;
using NATS.Client.JetStream;
using NATS.Net;

internal static class NatsExtensions
{
  public static IHostApplicationBuilder AddNats(this IHostApplicationBuilder builder, Func<NatsOpts, NatsOpts>? options = null)
  {
    //Create defaults for Nats options
    NatsOpts opts = NatsOpts.Default;
    if (options is not null)
      //If Func is provided, use it to override the defaults
      opts = options(opts);

    //Use Nats own extension to register NatsConnection and NatsConnectionPool
    _ = builder.Services.AddNats(configureOpts: (opts) => opts);

    //Additional register NATS client and JetStream context
    _ = builder.Services.AddSingleton<INatsClient>(sp => new NatsClient(opts));
    _ = builder.Services.AddSingleton(sp => sp.GetRequiredService<INatsClient>()
        .CreateJetStreamContext());

    //Register the NatsListenerService
    builder.Services.AddTransient<INatsListenerService<string>, NatsListenerService<string>>();

    return builder;
  }
}
