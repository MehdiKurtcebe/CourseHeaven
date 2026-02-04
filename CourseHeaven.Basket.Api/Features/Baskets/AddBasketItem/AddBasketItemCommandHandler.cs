using System.Text.Json;
using CourseHeaven.Basket.Api.Const;
using CourseHeaven.Basket.Api.Features.Baskets.Dtos;
using CourseHeaven.Shared;
using CourseHeaven.Shared.Services;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace CourseHeaven.Basket.Api.Features.Baskets.AddBasketItem;

public class AddBasketItemCommandHandler(IDistributedCache distributedCache, IIdentityService identityService)
    : IRequestHandler<AddBasketItemCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(AddBasketItemCommand request, CancellationToken cancellationToken)
    {
        var newBasketItem = new BasketItemDto(
            request.CourseId,
            request.CourseName,
            request.CoursePrice,
            null,
            request.CourseImageUrl
        );

        var userId = identityService.UserId;
        var cacheKey = string.Format(BasketConst.BasketCacheKey, userId);
        var basket = await distributedCache.GetStringAsync(cacheKey, cancellationToken);

        BasketDto? basketDto;
        if (string.IsNullOrEmpty(basket))
        {
            basketDto = new BasketDto(userId, [newBasketItem]);
            await SetCacheAsync(cacheKey, basketDto, cancellationToken);
            return ServiceResult.SuccessAsNoContent();
        }

        basketDto = JsonSerializer.Deserialize<BasketDto>(basket) ?? new BasketDto(userId, []);

        var existingBasketItem = basketDto.Items.FirstOrDefault(i => i.CourseId == request.CourseId);
        if (existingBasketItem is not null)
            basketDto.Items.Remove(existingBasketItem); // TODO: return conflict if item already exists

        basketDto.Items.Add(newBasketItem);

        await SetCacheAsync(cacheKey, basketDto, cancellationToken);
        return ServiceResult.SuccessAsNoContent();
    }

    private async Task SetCacheAsync(string cacheKey, BasketDto basketDto, CancellationToken cancellationToken)
    {
        var basket = JsonSerializer.Serialize(basketDto);
        await distributedCache.SetStringAsync(cacheKey, basket, cancellationToken);
    }
}