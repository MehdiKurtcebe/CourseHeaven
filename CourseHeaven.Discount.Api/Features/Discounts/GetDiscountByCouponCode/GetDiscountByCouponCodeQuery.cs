namespace CourseHeaven.Discount.Api.Features.Discounts.GetDiscountByCouponCode;

public record GetDiscountByCouponCodeQuery(string CouponCode)
    : IRequestByServiceResult<GetDiscountByCouponCodeQueryResponse>;