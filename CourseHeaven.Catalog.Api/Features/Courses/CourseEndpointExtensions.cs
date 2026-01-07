using CourseHeaven.Catalog.Api.Features.Courses.Create;

namespace CourseHeaven.Catalog.Api.Features.Courses;

public static class CourseEndpointExtensions
{
    public static void AddCourseGroupEndpointExtension(this WebApplication app)
    {
        app.MapGroup("api/courses")
            .WithTags("Courses")
            .CreateCourseGroupItemEndpoint();
    }
}