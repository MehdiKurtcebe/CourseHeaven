using CourseHeaven.Order.Application.Features.Orders.CreateOrder;
using CourseHeaven.Shared.Extensions;
using CourseHeaven.Shared.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CourseHeaven.Order.Api.Endpoints.Orders;

public static class CreateOrderEndpoint
{
    public static RouteGroupBuilder CreateOrderGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/",
                async ([FromBody] CreateOrderCommand command, [FromServices] IMediator mediator) =>
                (await mediator.Send(command)).ToGenericResult())
            .WithName("CreateOrder")
            .MapToApiVersion(1, 0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .AddEndpointFilter<ValidationFilter<CreateOrderCommand>>();

        return group;
    }
}