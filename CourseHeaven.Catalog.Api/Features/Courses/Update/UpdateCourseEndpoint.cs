using CourseHeaven.Shared.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CourseHeaven.Catalog.Api.Features.Courses.Update;

public static class UpdateCourseEndpoint
{
    public static RouteGroupBuilder UpdateCourseGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapPut("/",
                async (UpdateCourseCommand command, IMediator mediator) =>
                    (await mediator.Send(command)).ToGenericResult())
            .WithName("UpdateCourse")
            .MapToApiVersion(1, 0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .AddEndpointFilter<ValidationFilter<UpdateCourseCommand>>();

        return group;
    }
}