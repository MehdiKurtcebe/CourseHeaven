using CourseHeaven.Shared.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CourseHeaven.Payment.Api.Features.Payments.GetStatus;

public static class GetPaymentStatusQueryEndpoint
{
    public static RouteGroupBuilder GetPaymentStatusGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/status/{orderCode}",
                async ([FromServices] IMediator mediator, string orderCode) =>
                (await mediator.Send(new GetPaymentStatusQuery(orderCode))).ToGenericResult())
            .WithName("GetPaymentStatus")
            .MapToApiVersion(1, 0)
            .Produces(StatusCodes.Status200OK)
            .RequireAuthorization("ClientCredential");

        return group;
    }
}