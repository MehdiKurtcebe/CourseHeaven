using CourseHeaven.Shared;

namespace CourseHeaven.Payment.Api.Features.Payments.Create;

public record CreatePaymentCommand(
    string OrderCode,
    string CardNumber,
    string CardHolderName,
    string CardExpirationDate,
    string CardSecurityCode,
    decimal Amount) : IRequestByServiceResult<Guid>;