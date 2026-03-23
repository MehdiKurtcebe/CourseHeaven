using System.Net;
using CourseHeaven.Discount.Api.Repositories;
using CourseHeaven.Shared.Services;

namespace CourseHeaven.Discount.Api.Features.Discounts.GetDiscountByCouponCode;

public class GetDiscountByCouponCodeQueryHandler(AppDbContext context)
    : IRequestHandler<GetDiscountByCouponCodeQuery, ServiceResult<GetDiscountByCouponCodeQueryResponse>>
{
    public async Task<ServiceResult<GetDiscountByCouponCodeQueryResponse>> Handle(GetDiscountByCouponCodeQuery request,
        CancellationToken cancellationToken)
    {
        var discount = await context.Discounts
            .SingleOrDefaultAsync(d => d.CouponCode == request.CouponCode, cancellationToken);
        if (discount is null)
            return ServiceResult<GetDiscountByCouponCodeQueryResponse>.Error("Discount not found.",
                HttpStatusCode.NotFound);
        if (discount.ExpireAt < DateTimeOffset.UtcNow)
            return ServiceResult<GetDiscountByCouponCodeQueryResponse>.Error("Discount has expired.",
                HttpStatusCode.BadRequest);

        return ServiceResult<GetDiscountByCouponCodeQueryResponse>.SuccessAsOk(
            new GetDiscountByCouponCodeQueryResponse(discount.CouponCode, discount.DiscountRate));
    }
}