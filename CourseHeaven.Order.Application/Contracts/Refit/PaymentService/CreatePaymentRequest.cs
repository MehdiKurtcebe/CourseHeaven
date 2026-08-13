namespace CourseHeaven.Order.Application.Contracts.Refit.PaymentService;

public record CreatePaymentRequest(
    string OrderCode,
    string CardNumber,
    string CardHolderName,
    string CardExpirationDate,
    string CardSecurityCode,
    decimal Amount);