using CourseHeaven.Discount.Api.Repositories;

namespace CourseHeaven.Discount.Api.Features.Discounts;

public class Discount : BaseEntity
{
    public Guid UserId { get; set; }
    public decimal DiscountRate { get; set; }
    public string CouponCode { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset ExpireAt { get; set; }
}