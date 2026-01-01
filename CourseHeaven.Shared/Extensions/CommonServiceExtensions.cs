using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace CourseHeaven.Shared.Extensions;

public static class CommonServiceExtensions
{
    public static IServiceCollection AddCommonServiceExtension(this IServiceCollection services, Type assembly)
    {
        services.AddHttpContextAccessor();
        services.AddMediatR(x => x.RegisterServicesFromAssemblyContaining(assembly));

        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining(assembly);
        
        services.AddAutoMapper(_ => {}, assembly);

        return services;
    }
}