using CourseHeaven.Shared.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CourseHeaven.Payment.Api.Features.Payments.Create;

public static class CreatePaymentEndpoint
{
    public static RouteGroupBuilder CreatePaymentGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/", async (CreatePaymentCommand command, IMediator mediator) =>
                (await mediator.Send(command)).ToGenericResult())
            .WithName("CreatePayment")
            .MapToApiVersion(1, 0)
            .Produces<Guid>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .RequireAuthorization("Password");

        return group;
    }
}