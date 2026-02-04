using System.Net;
using System.Text.Json;
using CourseHeaven.Basket.Api.Const;
using CourseHeaven.Basket.Api.Features.Baskets.Dtos;
using CourseHeaven.Shared;
using CourseHeaven.Shared.Extensions;
using CourseHeaven.Shared.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace CourseHeaven.Basket.Api.Features.Baskets.DeleteBasketItem;

public record DeleteBasketItemCommand(Guid CourseId) : IRequestByServiceResult;

public class DeleteBasketItemCommandHandler(IDistributedCache distributedCache, IIdentityService identityService)
    : IRequestHandler<DeleteBasketItemCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(DeleteBasketItemCommand request, CancellationToken cancellationToken)
    {
        var userId = identityService.UserId;
        var cacheKey = string.Format(BasketConst.BasketCacheKey, userId);
        var basket = await distributedCache.GetStringAsync(cacheKey, cancellationToken);

        if (string.IsNullOrEmpty(basket))
            return ServiceResult.Error("Basket not found", HttpStatusCode.NotFound);

        var basketDto = JsonSerializer.Deserialize<BasketDto>(basket) ?? new BasketDto(userId, []);
        var basketItemToDelete = basketDto.Items.FirstOrDefault(i => i.CourseId == request.CourseId);
        if (basketItemToDelete is null)
            return ServiceResult.Error("Basket item not found", HttpStatusCode.NotFound);

        basketDto.Items.Remove(basketItemToDelete);
        var updatedBasket = JsonSerializer.Serialize(basketDto);
        await distributedCache.SetStringAsync(cacheKey, updatedBasket, cancellationToken);

        return ServiceResult.SuccessAsNoContent();
    }
}

public static class DeleteBasketItemEndpoint
{
    public static RouteGroupBuilder DeleteBasketItemGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapDelete("/items/{courseId:guid}",
                async (IMediator mediator, Guid courseId) =>
                    (await mediator.Send(new DeleteBasketItemCommand(courseId))).ToGenericResult())
            .WithName("DeleteBasketItem")
            .MapToApiVersion(1, 0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        return group;
    }
}