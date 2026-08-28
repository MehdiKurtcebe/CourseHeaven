namespace CourseHeaven.Web.Dto;

public record CourseDto(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    string? ImageUrl,
    DateTimeOffset CreatedAt,
    CategoryDto Category,
    FeatureDto Feature);