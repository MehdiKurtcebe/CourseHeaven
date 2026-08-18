using CourseHeaven.Web.Options;
using Microsoft.Extensions.Options;

namespace CourseHeaven.Web.Extensions;

public static class OptionsExtensions
{
    public static IServiceCollection AddOptionsExtension(this IServiceCollection services)
    {
        services.AddOptions<IdentityOptions>().BindConfiguration(nameof(IdentityOptions)).ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddSingleton<IdentityOptions>(sp => sp.GetRequiredService<IOptions<IdentityOptions>>().Value);

        services.AddOptions<GatewayOptions>().BindConfiguration(nameof(GatewayOptions)).ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddSingleton<GatewayOptions>(sp => sp.GetRequiredService<IOptions<GatewayOptions>>().Value);
        return services;
    }
}