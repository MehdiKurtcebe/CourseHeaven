using CourseHeaven.Catalog.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CourseHeaven.Catalog.Api.Features.Courses.Delete;

public record DeleteCourseCommand(Guid Id) : IRequestByServiceResult;

public class DeleteCourseHandler(AppDbContext context)
    : IRequestHandler<DeleteCourseCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(DeleteCourseCommand request,
        CancellationToken cancellationToken)
    {
        var course = await context.Courses.FindAsync([request.Id], cancellationToken);
        if (course == null)
            return ServiceResult.ErrorAsNotFound();

        context.Courses.Remove(course);
        await context.SaveChangesAsync(cancellationToken);

        return ServiceResult.SuccessAsNoContent();
    }
}

public static class DeleteCourseEndpoint
{
    public static RouteGroupBuilder DeleteCourseGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapDelete("/{id:guid}",
                async (IMediator mediator, Guid id) =>
                    (await mediator.Send(new DeleteCourseCommand(id))).ToGenericResult())
            .WithName("DeleteCourse")
            .MapToApiVersion(1, 0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .RequireAuthorization(policyNames: "InstructorPolicy");

        return group;
    }
}