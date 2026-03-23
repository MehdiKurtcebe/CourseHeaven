using Microsoft.AspNetCore.Mvc;

namespace CourseHeaven.Discount.Api.Features.Discounts.GetDiscountByCouponCode;

public static class GetDiscountByCouponCodeEndpoint
{
    public static RouteGroupBuilder GetDiscountByCouponCodeGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/{code:maxlength(100)}",
                async (string code, IMediator mediator) =>
                    (await mediator.Send(new GetDiscountByCouponCodeQuery(code))).ToGenericResult())
            .WithName("GetDiscountByCouponCode")
            .MapToApiVersion(1, 0)
            .Produces<GetDiscountByCouponCodeQueryResponse>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        return group;
    }
}