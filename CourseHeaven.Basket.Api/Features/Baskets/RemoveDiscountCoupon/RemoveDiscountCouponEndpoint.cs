namespace CourseHeaven.Basket.Api.Features.Baskets.RemoveDiscountCoupon;

public record RemoveDiscountCouponCommand : IRequestByServiceResult;

public class RemoveDiscountCouponCommandHandler(BasketService basketService)
    : IRequestHandler<RemoveDiscountCouponCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(RemoveDiscountCouponCommand request, CancellationToken cancellationToken)
    {
        var basketJson = await basketService.GetBasketFromCacheAsync(cancellationToken);
        if (string.IsNullOrEmpty(basketJson))
            return ServiceResult.Error("Basket not found", HttpStatusCode.NotFound);

        var basket = JsonSerializer.Deserialize<Data.Basket>(basketJson);
        if (basket == null)
            return ServiceResult.Error("Basket not found", HttpStatusCode.NotFound);

        basket.ClearDiscount();
        await basketService.CreateBasketCacheAsync(basket, cancellationToken);
        return ServiceResult.SuccessAsNoContent();
    }
}

public static class RemoveDiscountCouponEndpoint
{
    public static RouteGroupBuilder RemoveDiscountCouponGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapDelete("/discounts/remove",
                async (IMediator mediator) =>
                    (await mediator.Send(new RemoveDiscountCouponCommand())).ToGenericResult())
            .WithName("RemoveDiscountCoupon")
            .MapToApiVersion(1, 0)
            .Produces<ServiceResult>(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound);

        return group;
    }
}