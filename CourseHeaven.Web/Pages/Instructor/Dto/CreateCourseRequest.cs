namespace CourseHeaven.Web.Pages.Instructor.Dto;

public record CreateCourseRequest(
    string Name,
    string Description,
    decimal Price,
    IFormFile? Image,
    Guid CategoryId);