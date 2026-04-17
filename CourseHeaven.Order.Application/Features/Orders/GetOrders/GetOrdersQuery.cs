using CourseHeaven.Shared;

namespace CourseHeaven.Order.Application.Features.Orders.GetOrders;

public record GetOrdersQuery : IRequestByServiceResult<List<GetOrdersResponse>>;