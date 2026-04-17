namespace CourseHeaven.Basket.Api.Features.Baskets.ApplyDiscountCoupon;

public static class ApplyDiscountCouponEndpoint
{
    public static RouteGroupBuilder ApplyDiscountCouponGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapPut("/discounts/apply",
                async (ApplyDiscountCouponCommand command, IMediator mediator) =>
                    (await mediator.Send(command)).ToGenericResult())
            .WithName("ApplyDiscountCoupon")
            .MapToApiVersion(1, 0)
            .Produces<ServiceResult>(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .AddEndpointFilter<ValidationFilter<ApplyDiscountCouponCommand>>();

        return group;
    }
}