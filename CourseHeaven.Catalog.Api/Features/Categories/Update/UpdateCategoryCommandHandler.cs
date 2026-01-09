using CourseHeaven.Catalog.Api.Repositories;

namespace CourseHeaven.Catalog.Api.Features.Categories.Update;

public class UpdateCategoryCommandHandler(AppDbContext context) : IRequestHandler<UpdateCategoryCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await context.Categories.FindAsync([request.Id], cancellationToken);
        if (category == null)
            return ServiceResult.ErrorAsNotFound();

        category.Name = request.Name;

        context.Categories.Update(category);
        await context.SaveChangesAsync(cancellationToken);

        return ServiceResult.SuccessAsNoContent();
    }
}