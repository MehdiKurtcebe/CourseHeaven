using System.Net;
using AutoMapper;
using CourseHeaven.Catalog.Api.Features.Categories.Dtos;
using CourseHeaven.Catalog.Api.Repositories;
using CourseHeaven.Shared;
using CourseHeaven.Shared.Extensions;
using MediatR;

namespace CourseHeaven.Catalog.Api.Features.Categories.GetById;

public record GetCategoryByIdQuery(Guid Id) : IRequest<ServiceResult<CategoryDto>>;

public class GetCategoryByIdHandler(AppDbContext context, IMapper mapper)
    : IRequestHandler<GetCategoryByIdQuery, ServiceResult<CategoryDto>>
{
    public async Task<ServiceResult<CategoryDto>> Handle(GetCategoryByIdQuery request,
        CancellationToken cancellationToken)
    {
        var category = await context.Categories.FindAsync([request.Id], cancellationToken);
        if (category == null)
        {
            return ServiceResult<CategoryDto>.Error("Category not found",
                $"The category with ID {request.Id} was not found.", HttpStatusCode.NotFound);
        }

        var categoryAsDto = mapper.Map<CategoryDto>(category);
        return ServiceResult<CategoryDto>.SuccessAsOk(categoryAsDto);
    }
}

public static class GetCategoryByIdEndpoint
{
    public static RouteGroupBuilder GetCategoryByIdGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/{id:guid}",
            async (IMediator mediator, Guid id) =>
                (await mediator.Send(new GetCategoryByIdQuery(id))).ToGenericResult());

        return group;
    }
}