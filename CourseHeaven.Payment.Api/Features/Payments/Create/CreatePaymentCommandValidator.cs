using FluentValidation;

namespace CourseHeaven.Payment.Api.Features.Payments.Create;

public class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
{
    public CreatePaymentCommandValidator()
    {
        RuleFor(c => c.OrderCode).NotEmpty().WithMessage("{PropertyName} is required.");
        RuleFor(c => c.CardNumber).NotEmpty().WithMessage("{PropertyName} is required.")
            .CreditCard().WithMessage("{PropertyName} must be a valid credit card number.");
        RuleFor(c => c.CardHolderName).NotEmpty().WithMessage("{PropertyName} is required.");
        RuleFor(c => c.CardExpirationDate).NotEmpty().WithMessage("{PropertyName} is required.");
        RuleFor(c => c.CardSecurityCode).NotEmpty().WithMessage("{PropertyName} is required.");
        RuleFor(c => c.Amount).NotEmpty().WithMessage("{PropertyName} is required.")
            .GreaterThan(0).WithMessage("{PropertyName} must be greater than zero.")
            .PrecisionScale(18, 2, true)
            .WithMessage("{PropertyName} must have a maximum of 18 digits in total and 2 decimal places.");
    }
}