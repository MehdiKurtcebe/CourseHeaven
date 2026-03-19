using Asp.Versioning.Builder;
using CourseHeaven.Basket.Api.Features.Baskets.AddBasketItem;
using CourseHeaven.Basket.Api.Features.Baskets.ApplyDiscountCoupon;
using CourseHeaven.Basket.Api.Features.Baskets.DeleteBasketItem;
using CourseHeaven.Basket.Api.Features.Baskets.GetBasket;
using CourseHeaven.Basket.Api.Features.Baskets.RemoveDiscountCoupon;

namespace CourseHeaven.Basket.Api.Features.Baskets;

public static class BasketEndpointExtensions
{
    public static void AddBasketGroupEndpointExtension(this WebApplication app, ApiVersionSet apiVersionSet)
    {
        app.MapGroup("api/v{version:apiVersion}/baskets")
            .WithTags("Baskets")
            .WithApiVersionSet(apiVersionSet)
            .AddBasketItemGroupItemEndpoint()
            .DeleteBasketItemGroupItemEndpoint()
            .GetBasketGroupItemEndpoint()
            .ApplyDiscountCouponGroupItemEndpoint()
            .RemoveDiscountCouponGroupItemEndpoint();
    }
}