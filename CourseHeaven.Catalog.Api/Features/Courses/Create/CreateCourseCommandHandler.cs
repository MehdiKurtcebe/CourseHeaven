using CourseHeaven.Bus.Commands;
using CourseHeaven.Catalog.Api.Repositories;

namespace CourseHeaven.Catalog.Api.Features.Courses.Create;

public class CreateCourseCommandHandler(AppDbContext context, IMapper mapper, IPublishEndpoint publishEndpoint)
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
        course.Feature = new Feature { EducatorFullName = "" }; // TODO: Handle feature properly in the future

        context.Courses.Add(course);
        await context.SaveChangesAsync(cancellationToken);

        if (request.Image is not null)
        {
            using var stream = new MemoryStream();
            await request.Image.CopyToAsync(stream, cancellationToken);

            var imageBytes = stream.ToArray();

            var uploadCourseImageCommand = new UploadCourseImageCommand(course.Id, imageBytes, request.Image.FileName);

            await publishEndpoint.Publish(uploadCourseImageCommand, cancellationToken);
        }

        return ServiceResult<CreateCourseResponse>.SuccessAsCreated(new CreateCourseResponse(course.Id),
            $"/api/courses/{course.Id}");
    }
}