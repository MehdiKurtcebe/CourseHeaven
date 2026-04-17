using FluentValidation;

namespace CourseHeaven.Order.Application.Features.Orders.CreateOrder;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.OrderItems).NotEmpty().WithMessage("{PropertyName} must contain at least one item.");

        RuleForEach(x => x.OrderItems).SetValidator(new OrderItemDtoValidator());

        RuleFor(x => x.InvoiceAddress).NotEmpty().WithMessage("{PropertyName} is required.")
            .MaximumLength(500).WithMessage("{PropertyName} must not exceed 500 characters.");

        RuleFor(x => x.PaymentInfo).NotNull().WithMessage("{PropertyName} is required.")
            .SetValidator(new PaymentDtoValidator());

        RuleFor(x => x.DiscountRate).GreaterThanOrEqualTo(0)
            .WithMessage("{PropertyName} must be greater than or equal to zero.")
            .LessThanOrEqualTo(1).WithMessage("{PropertyName} must be less than or equal to one.")
            .PrecisionScale(3, 3, true)
            .WithMessage("{PropertyName} must have a maximum of 3 digits in total and 3 decimal places.");
    }
}

public class PaymentDtoValidator : AbstractValidator<PaymentDto>
{
    public PaymentDtoValidator()
    {
        RuleFor(x => x.CardNumber).NotEmpty().WithMessage("{PropertyName} is required.")
            .CreditCard().WithMessage("{PropertyName} must be a valid credit card number.");

        RuleFor(x => x.CardHolderName).NotEmpty().WithMessage("{PropertyName} is required.");

        RuleFor(x => x.Expiration).NotEmpty().WithMessage("{PropertyName} is required.");

        RuleFor(x => x.Cvc).NotEmpty().WithMessage("{PropertyName} is required.");

        RuleFor(x => x.Amount).NotEmpty().WithMessage("{PropertyName} is required.")
            .GreaterThan(0).WithMessage("{PropertyName} must be greater than zero.")
            .PrecisionScale(18, 2, true)
            .WithMessage("{PropertyName} must have a maximum of 18 digits in total and 2 decimal places.");
    }
}

public class OrderItemDtoValidator : AbstractValidator<OrderItemDto>
{
    public OrderItemDtoValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("{PropertyName} is required.");

        RuleFor(x => x.ProductName).NotEmpty().WithMessage("{PropertyName} is required.")
            .MaximumLength(200).WithMessage("{PropertyName} must not exceed 200 characters.");

        RuleFor(x => x.ProductPrice).NotEmpty().WithMessage("{PropertyName} is required.")
            .GreaterThan(0).WithMessage("{PropertyName} must be greater than zero.")
            .PrecisionScale(18, 2, true)
            .WithMessage("{PropertyName} must have a maximum of 18 digits in total and 2 decimal places.");
    }
}