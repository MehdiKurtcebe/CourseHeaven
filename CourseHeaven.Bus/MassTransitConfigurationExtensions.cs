using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CourseHeaven.Bus;

public static class MassTransitConfigurationExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddMassTransitExtension(IConfiguration configuration)
        {
            var busOptions = configuration.GetSection(nameof(BusOptions)).Get<BusOptions>()!;

            services.AddMassTransit(configurator =>
            {
                configurator.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(new Uri($"rabbitmq://{busOptions.Address}:{busOptions.Port}"), h =>
                    {
                        h.Username(busOptions.Username);
                        h.Password(busOptions.Password);
                    });

                    cfg.ConfigureEndpoints(ctx);
                });
            });

            return services;
        }

        public IServiceCollection AddMassTransitExtension(IConfiguration configuration, Type consumer, string queueName)
        {
            var busOptions = configuration.GetSection(nameof(BusOptions)).Get<BusOptions>()!;

            services.AddMassTransit(configurator =>
            {
                configurator.AddConsumer(consumer);

                configurator.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(new Uri($"rabbitmq://{busOptions.Address}:{busOptions.Port}"), h =>
                    {
                        h.Username(busOptions.Username);
                        h.Password(busOptions.Password);
                    });

                    cfg.ReceiveEndpoint(queueName, e => { e.ConfigureConsumer(ctx, consumer); });
                });
            });

            return services;
        }
    }
}