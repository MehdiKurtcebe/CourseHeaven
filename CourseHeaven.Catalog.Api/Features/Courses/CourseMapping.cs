using CourseHeaven.Catalog.Api.Features.Courses.Create;

namespace CourseHeaven.Catalog.Api.Features.Courses;

public class CourseMapping : Profile
{
    public CourseMapping()
    {
        CreateMap<CreateCourseCommand, Course>();
    }
}