using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CourseHeaven.Bus;

public static class MassTransitConfigurationExtensions
{
    public static IServiceCollection AddMassTransitExtension(this IServiceCollection services,
        IConfiguration configuration)
    {
        var busOptions = (configuration.GetSection(nameof(BusOptions)).Get<BusOptions>())!;

        services.AddMassTransit(configurator =>
        {
            configurator.UsingRabbitMq((_, cfg) =>
            {
                cfg.Host(new Uri($"rabbitmq://{busOptions.Address}:{busOptions.Port}"), h =>
                {
                    h.Username(busOptions.Username);
                    h.Password(busOptions.Password);
                });
            });
        });

        return services;
    }
}