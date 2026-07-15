using CourseHeaven.Shared;

namespace CourseHeaven.File.Api.Features.File.Delete;

public record DeleteFileCommand(string FileName) : IRequestByServiceResult;