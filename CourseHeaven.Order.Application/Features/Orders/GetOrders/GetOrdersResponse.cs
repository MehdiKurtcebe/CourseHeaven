using CourseHeaven.Order.Application.Features.Orders.CreateOrder;

namespace CourseHeaven.Order.Application.Features.Orders.GetOrders;

public record GetOrdersResponse(DateTimeOffset CreatedAt, decimal TotalPrice, List<OrderItemDto> Items);