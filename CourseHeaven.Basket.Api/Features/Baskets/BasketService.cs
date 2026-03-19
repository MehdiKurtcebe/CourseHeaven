using System.Text.Json;
using CourseHeaven.Basket.Api.Const;
using CourseHeaven.Shared.Services;
using Microsoft.Extensions.Caching.Distributed;

namespace CourseHeaven.Basket.Api.Features.Baskets;

public class BasketService(IIdentityService identityService, IDistributedCache distributedCache)
{
    private string GetCacheKey()
    {
        return string.Format(BasketConst.BasketCacheKey, identityService.UserId);
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
}