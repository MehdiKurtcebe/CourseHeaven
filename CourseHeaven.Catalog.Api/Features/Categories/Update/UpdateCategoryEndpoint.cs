using CourseHeaven.Shared.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CourseHeaven.Catalog.Api.Features.Categories.Update;

public static class UpdateCategoryEndpoint
{
    public static RouteGroupBuilder UpdateCategoryGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapPut("/",
                async (UpdateCategoryCommand command, IMediator mediator) =>
                    (await mediator.Send(command)).ToGenericResult())
            .WithName("UpdateCategory")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .AddEndpointFilter<ValidationFilter<UpdateCategoryCommand>>();

        return group;
    }
}