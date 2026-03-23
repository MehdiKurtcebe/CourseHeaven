using Asp.Versioning.Builder;
using CourseHeaven.Discount.Api.Features.Discounts.CreateDiscount;
using CourseHeaven.Discount.Api.Features.Discounts.GetDiscountByCouponCode;

namespace CourseHeaven.Discount.Api.Features.Discounts;

public static class DiscountEndpointExtensions
{
    public static void AddDiscountGroupEndpointExtension(this WebApplication app, ApiVersionSet apiVersionSet)
    {
        app.MapGroup("/api/v{version:apiVersion}/discounts")
            .WithTags("Discounts")
            .WithApiVersionSet(apiVersionSet)
            .CreateDiscountGroupItemEndpoint()
            .GetDiscountByCouponCodeGroupItemEndpoint();
    }
}