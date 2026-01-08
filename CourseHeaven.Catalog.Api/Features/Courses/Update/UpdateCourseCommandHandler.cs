using CourseHeaven.Catalog.Api.Repositories;

namespace CourseHeaven.Catalog.Api.Features.Courses.Update;

public class UpdateCourseCommandHandler(AppDbContext context)
    : IRequestHandler<UpdateCourseCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
    {
        var course = await context.Courses.FindAsync([request.Id], cancellationToken);
        if (course == null)
            return ServiceResult.ErrorAsNotFound();

        if (request.CategoryId != null)
        {
            var hasCategory = await context.Categories.AnyAsync(c => c.Id == request.CategoryId, cancellationToken);
            if (!hasCategory)
                return ServiceResult.Error("Category not found.",
                    $"The category with ID '{request.CategoryId}' does not exist.", HttpStatusCode.NotFound);
            course.CategoryId = request.CategoryId.Value;
        }

        if (request.Name != null) course.Name = request.Name;
        if (request.Description != null) course.Description = request.Description;
        if (request.Price != null) course.Price = request.Price.Value;
        if (request.ImageUrl != null) course.ImageUrl = request.ImageUrl;

        context.Courses.Update(course);
        await context.SaveChangesAsync(cancellationToken);

        return ServiceResult.SuccessAsNoContent();
    }
}