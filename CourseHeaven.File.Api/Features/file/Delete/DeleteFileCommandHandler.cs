using System.Net;
using CourseHeaven.Shared;
using MediatR;
using Microsoft.Extensions.FileProviders;

namespace CourseHeaven.File.Api.Features.File.Delete;

public class DeleteFileCommandHandler(IFileProvider fileProvider) : IRequestHandler<DeleteFileCommand, ServiceResult>
{
    public Task<ServiceResult> Handle(DeleteFileCommand request, CancellationToken cancellationToken)
    {
        var fileInfo = fileProvider.GetFileInfo(Path.Combine("files", request.FileName));
        if (!fileInfo.Exists)
            return Task.FromResult(ServiceResult.ErrorAsNotFound());

        try
        {
            System.IO.File.Delete(fileInfo.PhysicalPath!);
            return Task.FromResult(ServiceResult.SuccessAsNoContent());
        }
        catch (Exception ex)
        {
            return Task.FromResult(ServiceResult.Error($"Error deleting file: {ex.Message}",
                HttpStatusCode.InternalServerError));
        }
    }
}