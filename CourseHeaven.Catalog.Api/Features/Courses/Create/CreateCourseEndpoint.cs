using CourseHeaven.Shared.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CourseHeaven.Catalog.Api.Features.Courses.Create;

public static class CreateCourseEndpoint
{
    public static RouteGroupBuilder CreateCourseGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/",
                async ([FromForm] CreateCourseCommand command, IMediator mediator) =>
                (await mediator.Send(command)).ToGenericResult())
            .WithName("CreateCourse")
            .MapToApiVersion(1, 0)
            .Produces<CreateCourseResponse>(StatusCodes.Status201Created)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .AddEndpointFilter<ValidationFilter<CreateCourseCommand>>()
            .DisableAntiforgery()
            .RequireAuthorization(policyNames: "InstructorPolicy");

        return group;
    }
}