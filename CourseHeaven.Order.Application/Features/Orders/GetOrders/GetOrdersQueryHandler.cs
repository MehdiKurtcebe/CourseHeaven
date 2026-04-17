using AutoMapper;
using CourseHeaven.Order.Application.Contracts.Repositories;
using CourseHeaven.Order.Application.Features.Orders.CreateOrder;
using CourseHeaven.Shared;
using CourseHeaven.Shared.Services;
using MediatR;

namespace CourseHeaven.Order.Application.Features.Orders.GetOrders;

public class GetOrdersQueryHandler(IIdentityService identityService, IOrderRepository orderRepository, IMapper mapper)
    : IRequestHandler<GetOrdersQuery, ServiceResult<List<GetOrdersResponse>>>
{
    public async Task<ServiceResult<List<GetOrdersResponse>>> Handle(GetOrdersQuery request,
        CancellationToken cancellationToken)
    {
        var orders = await orderRepository.GetOrderByBuyerIdAsync(identityService.UserId, cancellationToken);

        var response = orders.Select(o =>
            new GetOrdersResponse(o.CreatedAt, o.TotalPrice, mapper.Map<List<OrderItemDto>>(o.OrderItems))).ToList();

        return ServiceResult<List<GetOrdersResponse>>.SuccessAsOk(response);
    }
}