namespace CourseHeaven.Discount.Api.Features.Discounts.CreateDiscount;

public record CreateDiscountCommand(Guid UserId, decimal DiscountRate, string CouponCode, DateTimeOffset ExpireAt)
    : IRequestByServiceResult;