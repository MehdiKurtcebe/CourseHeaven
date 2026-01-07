using CourseHeaven.Catalog.Api.Features.Courses.Create;
using CourseHeaven.Catalog.Api.Features.Courses.GetAll;

namespace CourseHeaven.Catalog.Api.Features.Courses;

public static class CourseEndpointExtensions
{
    public static void AddCourseGroupEndpointExtension(this WebApplication app)
    {
        app.MapGroup("api/courses")
            .WithTags("Courses")
            .CreateCourseGroupItemEndpoint()
            .GetAllCoursesGroupItemEndpoint();
    }
}