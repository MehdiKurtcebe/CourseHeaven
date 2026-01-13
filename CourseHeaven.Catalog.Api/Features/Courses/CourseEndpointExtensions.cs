using Asp.Versioning.Builder;
using CourseHeaven.Catalog.Api.Features.Courses.Create;
using CourseHeaven.Catalog.Api.Features.Courses.Delete;
using CourseHeaven.Catalog.Api.Features.Courses.GetAll;
using CourseHeaven.Catalog.Api.Features.Courses.GetAllByUserId;
using CourseHeaven.Catalog.Api.Features.Courses.GetById;
using CourseHeaven.Catalog.Api.Features.Courses.Update;

namespace CourseHeaven.Catalog.Api.Features.Courses;

public static class CourseEndpointExtensions
{
    public static void AddCourseGroupEndpointExtension(this WebApplication app, ApiVersionSet apiVersionSet)
    {
        app.MapGroup("api/v{version:apiVersion}/courses")
            .WithTags("Courses")
            .WithApiVersionSet(apiVersionSet)
            .CreateCourseGroupItemEndpoint()
            .GetAllCoursesGroupItemEndpoint()
            .GetAllCoursesByUserIdGroupItemEndpoint()
            .GetCourseByIdGroupItemEndpoint()
            .UpdateCourseGroupItemEndpoint()
            .DeleteCourseGroupItemEndpoint();
    }
}