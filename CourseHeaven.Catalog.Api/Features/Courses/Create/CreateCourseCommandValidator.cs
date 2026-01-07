namespace CourseHeaven.Catalog.Api.Features.Courses.Create;

public class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand>
{
    public CreateCourseCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MaximumLength(200).WithMessage("{PropertyName} must not exceed 200 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MaximumLength(1000).WithMessage("{PropertyName} must not exceed 1000 characters.");

        RuleFor(x => x.Price)
            .PrecisionScale(18, 2, true)
            .WithMessage("{PropertyName} must have maximum 18 digits in total and 2 decimal places.")
            .GreaterThanOrEqualTo(0).WithMessage("{PropertyName} must be greater than or equal to zero.");

        RuleFor(x => x.ImageUrl)
            .MaximumLength(500).WithMessage("{PropertyName} must not exceed 500 characters.");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("{PropertyName} is required.");
    }
}