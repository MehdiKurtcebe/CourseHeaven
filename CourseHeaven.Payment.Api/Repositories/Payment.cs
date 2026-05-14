using MassTransit;

namespace CourseHeaven.Payment.Api.Repositories;

public class Payment(Guid userId, string orderCode, decimal amount)
{
    public Guid Id { get; set; } = NewId.NextSequentialGuid();
    public Guid UserId { get; set; } = userId;
    public string OrderCode { get; set; } = orderCode;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public decimal Amount { get; set; } = amount;
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
}

public enum PaymentStatus
{
    Success = 1,
    Failed = 2,
    Pending = 3
}