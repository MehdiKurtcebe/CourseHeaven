namespace CourseHeaven.Catalog.Api.Features.Courses.Create;

public record CreateCourseCommand : IRequestByServiceResult<CreateCourseResponse>
{
    public string Name { get; init; } = null!;
    public string Description { get; init; } = null!;
    public decimal Price { get; init; }
    public IFormFile? Image { get; set; }
    public Guid CategoryId { get; init; }
}