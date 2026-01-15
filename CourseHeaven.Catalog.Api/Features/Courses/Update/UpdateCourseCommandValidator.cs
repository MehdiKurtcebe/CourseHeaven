namespace CourseHeaven.Catalog.Api.Features.Courses.Update;

public class UpdateCourseCommandValidator : AbstractValidator<UpdateCourseCommand>
{
    public UpdateCourseCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("{PropertyName} is required.");
        RuleFor(x => x.Name)
            .MinimumLength(1).WithMessage("{PropertyName} must be at least {MinLength} characters long.")
            .MaximumLength(200).WithMessage("{PropertyName} must not exceed 200 characters.");

        RuleFor(x => x.Description)
            .MinimumLength(1).WithMessage("{PropertyName} must be at least {MinLength} characters long.")
            .MaximumLength(1000).WithMessage("{PropertyName} must not exceed 1000 characters.");

        RuleFor(x => x.Price)
            .PrecisionScale(18, 2, true)
            .WithMessage("{PropertyName} must have maximum 18 digits in total and 2 decimal places.")
            .GreaterThanOrEqualTo(0).WithMessage("{PropertyName} must be greater than or equal to zero.");

        RuleFor(x => x.ImageUrl)
            .MaximumLength(1000).WithMessage("{PropertyName} must not exceed 500 characters.");
    }
}