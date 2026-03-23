namespace CourseHeaven.Basket.Api.Features.Baskets.ApplyDiscountCoupon;

public class ApplyDiscountCouponCommandHandler(BasketService basketService)
    : IRequestHandler<ApplyDiscountCouponCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(ApplyDiscountCouponCommand request, CancellationToken cancellationToken)
    {
        var basketJson = await basketService.GetBasketFromCacheAsync(cancellationToken);
        if (string.IsNullOrEmpty(basketJson))
            return ServiceResult.Error("Basket not found", HttpStatusCode.NotFound);

        var basket = JsonSerializer.Deserialize<Data.Basket>(basketJson);
        if (basket is null || basket.Items.Count == 0)
            return ServiceResult.Error("Basket is empty", HttpStatusCode.NotFound);

        basket.ApplyNewDiscount(request.CouponCode, request.DiscountRate);

        await basketService.CreateBasketCacheAsync(basket, cancellationToken);

        return ServiceResult.SuccessAsNoContent();
    }
}