namespace CourseHeaven.Web.Dto;

public record CreateCourseRequest(
    string Name,
    string Description,
    decimal Price,
    IFormFile? Image,
    Guid CategoryId);