namespace CourseHeaven.Web.Pages.Order.Dto;

public record CreateOrderRequest(
    decimal? DiscountRate,
    string InvoiceAddress,
    PaymentDto PaymentInfo,
    List<OrderItemDto> OrderItems);