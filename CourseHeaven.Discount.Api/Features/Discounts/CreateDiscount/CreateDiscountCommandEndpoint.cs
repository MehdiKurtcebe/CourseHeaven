using CourseHeaven.Shared.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CourseHeaven.Discount.Api.Features.Discounts.CreateDiscount;

public static class CreateDiscountCommandEndpoint
{
    public static RouteGroupBuilder CreateDiscountGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/",
                async (CreateDiscountCommand command, IMediator mediator) =>
                    (await mediator.Send(command)).ToGenericResult())
            .WithName("CreateDiscount")
            .MapToApiVersion(1, 0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .AddEndpointFilter<ValidationFilter<CreateDiscountCommand>>()
            .AllowAnonymous();

        return group;
    }
}