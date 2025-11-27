using Microsoft.Extensions.Options;

namespace CourseHeaven.Catalog.Api.Options;

public static class OptionsExtensions
{
    public static IServiceCollection AddOptionsExtension(this IServiceCollection services)
    {
        services.AddOptions<MongoOptions>().BindConfiguration(nameof(MongoOptions)).ValidateDataAnnotations()
            .ValidateOnStart();
        
        services.AddSingleton<MongoOptions>(sp => sp.GetRequiredService<IOptions<MongoOptions>>().Value);
        
        return services;
    }
}