namespace CourseHeaven.Payment.Api.Features.Payments.GetStatus;

public record GetPaymentStatusResponse(Guid? PaymentId, bool IsPaid);