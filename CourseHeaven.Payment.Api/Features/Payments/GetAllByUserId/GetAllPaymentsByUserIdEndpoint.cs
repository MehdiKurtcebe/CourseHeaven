using CourseHeaven.Shared.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CourseHeaven.Payment.Api.Features.Payments.GetAllByUserId;

public static class GetAllPaymentsByUserIdEndpoint
{
    public static RouteGroupBuilder GetAllPaymentsByUserIdGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/", async (IMediator mediator) =>
                (await mediator.Send(new GetAllPaymentsByUserIdQuery())).ToGenericResult())
            .WithName("GetAllPaymentsByUserId")
            .MapToApiVersion(1, 0)
            .Produces<List<GetAllPaymentsByUserIdResponse>>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);

        return group;
    }
}