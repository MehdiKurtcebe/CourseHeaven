using CourseHeaven.Shared;

namespace CourseHeaven.File.Api.Features.File.Upload;

public record UploadFileCommand(IFormFile File) : IRequestByServiceResult<UploadFileCommandResponse>;