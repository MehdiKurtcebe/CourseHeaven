using CourseHeaven.Catalog.Api.Features.Courses.Create;
using CourseHeaven.Catalog.Api.Features.Courses.GetAll;
using CourseHeaven.Catalog.Api.Features.Courses.GetById;

namespace CourseHeaven.Catalog.Api.Features.Courses;

public static class CourseEndpointExtensions
{
    public static void AddCourseGroupEndpointExtension(this WebApplication app)
    {
        app.MapGroup("api/courses")
            .WithTags("Courses")
            .CreateCourseGroupItemEndpoint()
            .GetAllCoursesGroupItemEndpoint()
            .GetCourseByIdGroupItemEndpoint();
    }
}