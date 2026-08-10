namespace CourseHeaven.Catalog.Api.Features.Courses.Create;

public record CreateCourseCommand(
    string Name,
    string Description,
    decimal Price,
    IFormFile? Image,
    Guid CategoryId) : IRequestByServiceResult<CreateCourseResponse>;