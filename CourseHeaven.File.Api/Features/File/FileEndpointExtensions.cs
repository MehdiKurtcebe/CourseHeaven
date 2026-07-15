using Asp.Versioning.Builder;
using CourseHeaven.File.Api.Features.File.Delete;
using CourseHeaven.File.Api.Features.File.Upload;

namespace CourseHeaven.File.Api.Features.File;

public static class FileEndpointExtensions
{
    public static void AddFileGroupEndpointExtension(this WebApplication app, ApiVersionSet apiVersionSet)
    {
        app.MapGroup("api/v{version:apiVersion}/files")
            .WithTags("Files")
            .WithApiVersionSet(apiVersionSet)
            .UploadFileGroupItemEndpoint()
            .DeleteFileGroupItemEndpoint()
            .RequireAuthorization();
    }
}