using CourseHeaven.Catalog.Api.Features.Categories.Dtos;
using CourseHeaven.Catalog.Api.Repositories;

namespace CourseHeaven.Catalog.Api.Features.Categories.GetAll;

public record GetAllCategoriesQuery : IRequestByServiceResult<List<CategoryDto>>;

public class GetAllCategoriesHandler(AppDbContext context, IMapper mapper)
    : IRequestHandler<GetAllCategoriesQuery, ServiceResult<List<CategoryDto>>>
{
    public async Task<ServiceResult<List<CategoryDto>>> Handle(GetAllCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var categories = await context.Categories.ToListAsync(cancellationToken);
        var categoriesAsDto = mapper.Map<List<CategoryDto>>(categories);

        return ServiceResult<List<CategoryDto>>.SuccessAsOk(categoriesAsDto);
    }
}

public static class GetAllCategoriesEndpoint
{
    public static RouteGroupBuilder GetAllCategoriesGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/",
                async (IMediator mediator) => (await mediator.Send(new GetAllCategoriesQuery())).ToGenericResult())
            .WithName("GetAllCategories")
            .MapToApiVersion(1, 0)
            .Produces<List<CategoryDto>>()
            .RequireAuthorization("ClientCredential");

        return group;
    }
}