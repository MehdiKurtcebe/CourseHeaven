using Microsoft.Extensions.DependencyInjection;

namespace CourseHeaven.Shared.Extensions;

public static class CommonServiceExtensions
{
    public static IServiceCollection AddCommonServiceExtension(this IServiceCollection services, Type assembly)
    {
        services.AddHttpContextAccessor();
        services.AddMediatR(x => x.RegisterServicesFromAssemblyContaining(assembly));

        return services;
    }
}