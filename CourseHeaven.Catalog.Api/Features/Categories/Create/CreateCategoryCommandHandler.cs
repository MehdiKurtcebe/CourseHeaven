using System.Net;
using CourseHeaven.Catalog.Api.Repositories;
using CourseHeaven.Shared;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CourseHeaven.Catalog.Api.Features.Categories.Create;

public class CreateCategoryCommandHandler(AppDbContext context)
    : IRequestHandler<CreateCategoryCommand, ServiceResult<CreateCategoryResponse>>
{
    public async Task<ServiceResult<CreateCategoryResponse>> Handle(CreateCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var categoryExists = await context.Categories.AnyAsync(x => x.Name == request.Name, cancellationToken);

        if (categoryExists)
        {
            return ServiceResult<CreateCategoryResponse>.Error("Category with the same name already exists.",
                $"The category '{request.Name}' already exists.", HttpStatusCode.BadRequest);
        }

        var category = new Category
        {
            Name = request.Name,
            Id = NewId.NextSequentialGuid()
        };

        await context.AddAsync(category, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return ServiceResult<CreateCategoryResponse>.SuccessAsCreated(new CreateCategoryResponse(category.Id),
            "<empty>");
    }
}