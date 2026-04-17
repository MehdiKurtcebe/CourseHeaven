using CourseHeaven.Order.Application.Features.Orders.GetOrders;
using CourseHeaven.Shared.Extensions;
using MediatR;

namespace CourseHeaven.Order.Api.Endpoints.Orders;

public static class GetOrdersEndpoint
{
    public static RouteGroupBuilder GetOrdersGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/", async (IMediator mediator) =>
                (await mediator.Send(new GetOrdersQuery())).ToGenericResult())
            .WithName("GetOrders")
            .MapToApiVersion(1, 0)
            .Produces<List<GetOrdersResponse>>();

        return group;
    }
}