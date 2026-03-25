using CourseHeaven.Shared.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CourseHeaven.File.Api.Features.File.Upload;

public static class UploadFileCommandEndpoint
{
    public static RouteGroupBuilder UploadFileGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/",
                async (IFormFile file, IMediator mediator) =>
                    (await mediator.Send(new UploadFileCommand(file))).ToGenericResult())
            .WithName("UploadFile")
            .MapToApiVersion(1, 0)
            .Produces<UploadFileCommandResponse>(StatusCodes.Status201Created)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .DisableAntiforgery();

        return group;
    }
}