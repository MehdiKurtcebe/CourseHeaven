namespace CourseHeaven.Catalog.Api.Features.Categories.Update;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("{PropertyName} is required.");
        RuleFor(x => x.Name)
            .MinimumLength(4).WithMessage("{PropertyName} must be at least {MinLength} characters long.")
            .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");
    }
}