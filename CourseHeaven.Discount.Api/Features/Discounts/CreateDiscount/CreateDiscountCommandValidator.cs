using FluentValidation;

namespace CourseHeaven.Discount.Api.Features.Discounts.CreateDiscount;

public class CreateDiscountCommandValidator : AbstractValidator<CreateDiscountCommand>
{
    public CreateDiscountCommandValidator()
    {
        RuleFor(c => c.UserId).NotEmpty().WithMessage("{PropertyName} is required.");
        RuleFor(c => c.DiscountRate).NotEmpty().WithMessage("{PropertyName} is required.")
            .GreaterThanOrEqualTo(0).LessThanOrEqualTo(1).WithMessage("{PropertyName} must be between 0 and 1.")
            .PrecisionScale(3, 3, true).WithMessage("{PropertyName} has precision 3.");
        RuleFor(c => c.CouponCode).NotEmpty().WithMessage("{PropertyName} is required.")
            .MaximumLength(100).WithMessage("{PropertyName} must not exceed {MaxLength} characters.");
        RuleFor(c => c.ExpireAt).NotEmpty().WithMessage("{PropertyName} is required.")
            .GreaterThan(DateTimeOffset.UtcNow).WithMessage("{PropertyName} must be in the future.");
    }
}