using CourseHeaven.Payment.Api.Repositories;

namespace CourseHeaven.Payment.Api.Features.Payments.GetAllByUserId;

public record GetAllPaymentsByUserIdResponse(
    Guid Id,
    string OrderCode,
    decimal Amount,
    DateTimeOffset CreatedAt,
    PaymentStatus Status);