using CourseHeaven.Catalog.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CourseHeaven.Catalog.Api.Features.Categories.Delete;

public record DeleteCategoryCommand(Guid Id) : IRequestByServiceResult;

public class DeleteCategoryHandler(AppDbContext context) : IRequestHandler<DeleteCategoryCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await context.Categories.FindAsync([request.Id], cancellationToken);
        if (category == null)
            return ServiceResult.ErrorAsNotFound();

        var hasCourses = await context.Courses.AnyAsync(c => c.CategoryId == request.Id, cancellationToken);
        if (hasCourses)
            return ServiceResult.Error("Conflict",
                "Cannot delete category because there are courses associated with it.",
                HttpStatusCode.Conflict);

        context.Categories.Remove(category);
        await context.SaveChangesAsync(cancellationToken);

        return ServiceResult.SuccessAsNoContent();
    }
}

public static class DeleteCategoryEndpoint
{
    public static RouteGroupBuilder DeleteCategoryGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapDelete("/{id:guid}",
                async (IMediator mediator, Guid id) =>
                    (await mediator.Send(new DeleteCategoryCommand(id))).ToGenericResult())
            .WithName("DeleteCategory")
            .MapToApiVersion(1, 0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status409Conflict);

        return group;
    }
}