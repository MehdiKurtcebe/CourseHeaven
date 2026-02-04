using Asp.Versioning.Builder;
using CourseHeaven.Basket.Api.Features.Baskets.AddBasketItem;

namespace CourseHeaven.Basket.Api.Features.Baskets;

public static class BasketEndpointExtensions
{
    public static void AddBasketGroupEndpointExtension(this WebApplication app, ApiVersionSet apiVersionSet)
    {
        app.MapGroup("api/v{version:apiVersion}/baskets")
            .WithTags("Baskets")
            .WithApiVersionSet(apiVersionSet)
            .AddBasketItemGroupItemEndpoint();
    }
}