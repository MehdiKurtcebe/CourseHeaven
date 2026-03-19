using System.Text.Json;
using CourseHeaven.Basket.Api.Data;
using CourseHeaven.Shared;
using CourseHeaven.Shared.Services;
using MediatR;

namespace CourseHeaven.Basket.Api.Features.Baskets.AddBasketItem;

public class AddBasketItemCommandHandler(IIdentityService identityService, BasketService basketService)
    : IRequestHandler<AddBasketItemCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(AddBasketItemCommand request, CancellationToken cancellationToken)
    {
        var newBasketItem = new BasketItem(
            request.CourseId,
            request.CourseName,
            request.CoursePrice,
            null,
            request.CourseImageUrl
        );

        var userId = identityService.UserId;
        var basketJson = await basketService.GetBasketFromCacheAsync(cancellationToken);

        Data.Basket? basket;
        if (string.IsNullOrEmpty(basketJson))
        {
            basket = new Data.Basket(userId, [newBasketItem]);
            await basketService.CreateBasketCacheAsync(basket, cancellationToken);
            return ServiceResult.SuccessAsNoContent();
        }

        basket = JsonSerializer.Deserialize<Data.Basket>(basketJson) ?? new Data.Basket(userId, []);

        var existingBasketItem = basket.Items.FirstOrDefault(item => item.CourseId == request.CourseId);
        if (existingBasketItem is not null)
            basket.Items.Remove(existingBasketItem); // TODO: return conflict if item already exists

        basket.Items.Add(newBasketItem);
        basket.ApplyAvailableDiscount();

        await basketService.CreateBasketCacheAsync(basket, cancellationToken);
        return ServiceResult.SuccessAsNoContent();
    }
}