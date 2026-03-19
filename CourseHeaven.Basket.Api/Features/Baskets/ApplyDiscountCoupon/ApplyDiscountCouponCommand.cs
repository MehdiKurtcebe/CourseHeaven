using CourseHeaven.Shared;

namespace CourseHeaven.Basket.Api.Features.Baskets.ApplyDiscountCoupon;

public record ApplyDiscountCouponCommand(string CouponCode, decimal DiscountRate) : IRequestByServiceResult;