using CourseHeaven.Shared;
using MediatR;

namespace CourseHeaven.Catalog.Api.Features.Categories.Create;

public record CreateCategoryCommand(string Name) : IRequestByServiceResult<CreateCategoryResponse>;