using CourseHeaven.Basket.Api.Const;
using Microsoft.Extensions.Caching.Distributed;

namespace CourseHeaven.Basket.Api.Features.Baskets;

public class BasketService(IIdentityService identityService, IDistributedCache distributedCache)
{
    private string GetCacheKey()
    {
        return string.Format(BasketConst.BasketCacheKey, identityService.UserId);
    }

    private string GetCacheKey(Guid userId)
    {
        return string.Format(BasketConst.BasketCacheKey, userId);
    }

    public Task<string?> GetBasketFromCacheAsync(CancellationToken cancellationToken)
    {
        return distributedCache.GetStringAsync(GetCacheKey(), cancellationToken);
    }

    public async Task CreateBasketCacheAsync(Data.Basket basket, CancellationToken cancellationToken)
    {
        var basketJson = JsonSerializer.Serialize(basket);
        await distributedCache.SetStringAsync(GetCacheKey(), basketJson, cancellationToken);
    }

    public async Task DeleteBasketAsync(Guid userId)
    {
        await distributedCache.RemoveAsync(GetCacheKey(userId));
    }
}