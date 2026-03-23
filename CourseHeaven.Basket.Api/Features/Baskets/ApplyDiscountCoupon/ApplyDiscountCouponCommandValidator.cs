namespace CourseHeaven.Basket.Api.Features.Baskets.ApplyDiscountCoupon;

public class ApplyDiscountCouponCommandValidator : AbstractValidator<ApplyDiscountCouponCommand>
{
    public ApplyDiscountCouponCommandValidator()
    {
        RuleFor(c => c.CouponCode).NotEmpty().WithMessage("{PropertyName} is required.");
        RuleFor(c => c.DiscountRate).GreaterThanOrEqualTo(0).LessThanOrEqualTo(1)
            .WithMessage("{PropertyName} must be between 0 and 1.");
    }
}