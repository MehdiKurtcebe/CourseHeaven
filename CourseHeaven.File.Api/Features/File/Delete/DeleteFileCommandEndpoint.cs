using CourseHeaven.Shared.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CourseHeaven.File.Api.Features.File.Delete;

public static class DeleteFileCommandEndpoint
{
    public static RouteGroupBuilder DeleteFileGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapDelete("",
                async ([FromBody] DeleteFileCommand command, IMediator mediator) =>
                (await mediator.Send(command)).ToGenericResult())
            .WithName("DeleteFile")
            .MapToApiVersion(1, 0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        return group;
    }
}