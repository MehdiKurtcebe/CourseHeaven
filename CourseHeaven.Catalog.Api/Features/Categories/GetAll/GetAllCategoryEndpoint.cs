using CourseHeaven.Catalog.Api.Features.Categories.Dtos;
using CourseHeaven.Catalog.Api.Repositories;

namespace CourseHeaven.Catalog.Api.Features.Categories.GetAll;

public class GetAllCategoryQuery : IRequestByServiceResult<List<CategoryDto>>;

public class GetAllCategoryHandler(AppDbContext context, IMapper mapper)
    : IRequestHandler<GetAllCategoryQuery, ServiceResult<List<CategoryDto>>>
{
    public async Task<ServiceResult<List<CategoryDto>>> Handle(GetAllCategoryQuery request,
        CancellationToken cancellationToken)
    {
        var categories = await context.Categories.ToListAsync(cancellationToken);
        var categoriesAsDto = mapper.Map<List<CategoryDto>>(categories);

        return ServiceResult<List<CategoryDto>>.SuccessAsOk(categoriesAsDto);
    }
}

public static class GetAllCategoryEndpoint
{
    public static RouteGroupBuilder GetAllCategoryGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/",
            async (IMediator mediator) => (await mediator.Send(new GetAllCategoryQuery())).ToGenericResult());

        return group;
    }
}