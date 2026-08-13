using CourseHeaven.Order.Application.Contracts.Refit.PaymentService;
using CourseHeaven.Shared.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Refit;

namespace CourseHeaven.Order.Application.Contracts.Refit;

public static class RefitConfigurationExtensions
{
    public static IServiceCollection AddRefitConfigurationExtension(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<AuthenticatedHttpClientHandler>();
        services.AddScoped<ClientAuthenticatedHttpClientHandler>();

        services.AddOptions<IdentityOptions>().BindConfiguration(nameof(IdentityOptions)).ValidateDataAnnotations()
            .ValidateOnStart();
        services.AddSingleton<IdentityOptions>(sp => sp.GetRequiredService<IOptions<IdentityOptions>>().Value);

        services.AddOptions<ClientSecretOptions>().BindConfiguration(nameof(ClientSecretOptions))
            .ValidateDataAnnotations().ValidateOnStart();
        services.AddSingleton<ClientSecretOptions>(sp => sp.GetRequiredService<IOptions<ClientSecretOptions>>().Value);

        services.AddRefitClient<IPaymentService>().ConfigureHttpClient(client =>
            {
                var addressUrlOptions = configuration.GetSection(nameof(AddressUrlOptions)).Get<AddressUrlOptions>();

                client.BaseAddress = new Uri(addressUrlOptions!.PaymentUrl);
            }).AddHttpMessageHandler<AuthenticatedHttpClientHandler>()
            .AddHttpMessageHandler<ClientAuthenticatedHttpClientHandler>();

        return services;
    }
}