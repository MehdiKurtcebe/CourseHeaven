using CourseHeaven.Catalog.Api.Features.Courses;
using CourseHeaven.Catalog.Api.Repositories;

namespace CourseHeaven.Catalog.Api.Features.Categories;

public class Category : BaseEntity
{
    public string Name { get; set; } = null!;

    public List<Course>? Courses { get; set; }
}