using System.Net;
using System.Text.Json;
using AutoMapper;
using CourseHeaven.Basket.Api.Dtos;
using CourseHeaven.Shared;
using CourseHeaven.Shared.Extensions;
using MediatR;

namespace CourseHeaven.Basket.Api.Features.Baskets.GetBasket;

public record GetBasketQuery : IRequestByServiceResult<BasketDto>;

public class GetBasketHandler(BasketService basketService, IMapper mapper)
    : IRequestHandler<GetBasketQuery, ServiceResult<BasketDto>>
{
    public async Task<ServiceResult<BasketDto>> Handle(GetBasketQuery request, CancellationToken cancellationToken)
    {
        var basketJson = await basketService.GetBasketFromCacheAsync(cancellationToken);
        if (string.IsNullOrEmpty(basketJson))
            return ServiceResult<BasketDto>.Error("Basket not found", HttpStatusCode.NotFound);

        var basket = JsonSerializer.Deserialize<Data.Basket>(basketJson);
        var basketDto = mapper.Map<BasketDto>(basket);

        return ServiceResult<BasketDto>.SuccessAsOk(basketDto);
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