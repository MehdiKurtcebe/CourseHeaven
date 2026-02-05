using System.Net;
using System.Text.Json;
using CourseHeaven.Basket.Api.Const;
using CourseHeaven.Basket.Api.Features.Baskets.Dtos;
using CourseHeaven.Shared;
using CourseHeaven.Shared.Extensions;
using CourseHeaven.Shared.Services;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace CourseHeaven.Basket.Api.Features.Baskets.GetBasket;

public record GetBasketQuery : IRequestByServiceResult<BasketDto>;

public class GetBasketHandler(IDistributedCache distributedCache, IIdentityService identityService)
    : IRequestHandler<GetBasketQuery, ServiceResult<BasketDto>>
{
    public async Task<ServiceResult<BasketDto>> Handle(GetBasketQuery request, CancellationToken cancellationToken)
    {
        var userId = identityService.UserId;
        var cacheKey = string.Format(BasketConst.BasketCacheKey, userId);
        var basketAsString = await distributedCache.GetStringAsync(cacheKey, cancellationToken);

        if (string.IsNullOrEmpty(basketAsString))
            return ServiceResult<BasketDto>.Error("Basket not found", HttpStatusCode.NotFound);

        var basketDto = JsonSerializer.Deserialize<BasketDto>(basketAsString);

        return ServiceResult<BasketDto>.SuccessAsOk(basketDto!);
    }
}

public static class GetBasketEndpoint
{
    public static RouteGroupBuilder GetBasketGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/user",
                async (IMediator mediator) => (await mediator.Send(new GetBasketQuery())).ToGenericResult())
            .WithName("GetBasket")
            .MapToApiVersion(1, 0)
            .Produces<BasketDto>()
            .ProducesProblem(StatusCodes.Status404NotFound);

        return group;
    }
}