namespace CourseHeaven.Basket.Api.Features.Baskets.AddBasketItem;

public class AddBasketItemCommandValidator : AbstractValidator<AddBasketItemCommand>
{
    public AddBasketItemCommandValidator()
    {
        RuleFor(x => x.CourseId)
            .NotEmpty().WithMessage("{PropertyName} is required.");

        RuleFor(x => x.CourseName)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MaximumLength(200).WithMessage("{PropertyName} must not exceed {MaxLength} characters.");

        RuleFor(x => x.CoursePrice)
            .PrecisionScale(18, 2, true)
            .WithMessage(
                "{PropertyName} must have maximum {ExpectedPrecision} digits in total and {ExpectedScale} decimal places.")
            .GreaterThanOrEqualTo(0).WithMessage("CoursePrice must be greater than or equal to {ComparisonValue}.");

        RuleFor(x => x.CourseImageUrl)
            .MaximumLength(1000).WithMessage("CourseImageUrl must not exceed {MaxLength} characters.");
    }
}