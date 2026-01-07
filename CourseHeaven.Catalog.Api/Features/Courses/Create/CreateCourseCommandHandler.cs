using CourseHeaven.Catalog.Api.Repositories;

namespace CourseHeaven.Catalog.Api.Features.Courses.Create;

public class CreateCourseCommandHandler(AppDbContext context, IMapper mapper)
    : IRequestHandler<CreateCourseCommand, ServiceResult<CreateCourseResponse>>
{
    public async Task<ServiceResult<CreateCourseResponse>> Handle(CreateCourseCommand request,
        CancellationToken cancellationToken)
    {
        var hasCategory = await context.Categories.AnyAsync(c => c.Id == request.CategoryId, cancellationToken);
        if (!hasCategory)
            return ServiceResult<CreateCourseResponse>.Error("Category not found.",
                $"The category with ID '{request.CategoryId}' does not exist.", HttpStatusCode.NotFound);

        var course = mapper.Map<Course>(request);
        course.Id = NewId.NextSequentialGuid();
        course.CreatedAt = DateTime.UtcNow;

        context.Courses.Add(course);
        await context.SaveChangesAsync(cancellationToken);

        return ServiceResult<CreateCourseResponse>.SuccessAsCreated(new CreateCourseResponse(course.Id),
            $"/api/courses/{course.Id}");
    }
}