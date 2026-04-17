using CourseHeaven.Shared;

namespace CourseHeaven.Order.Application.Features.Orders.CreateOrder;

public record CreateOrderCommand(
    List<OrderItemDto> OrderItems,
    string InvoiceAddress,
    PaymentDto PaymentInfo,
    decimal DiscountRate = 0) : IRequestByServiceResult;

public record PaymentDto(string CardNumber, string CardHolderName, string Expiration, string Cvc, decimal Amount);

public record OrderItemDto(Guid ProductId, string ProductName, decimal ProductPrice);