using System.Net;
using CourseHeaven.Web.Extensions;
using CourseHeaven.Web.Pages.Basket.Dto;
using CourseHeaven.Web.Pages.Basket.ViewModel;
using CourseHeaven.Web.Services.Refit;
using Refit;

namespace CourseHeaven.Web.Services;

public class BasketService(IBasketRefitService basketRefitService, IDiscountRefitService discountRefitService, ILogger<BasketService> logger)
{
    public async Task<ServiceResult> CreateOrUpdateBasketAsync(AddBasketRequest request)
    {
        var responseAsResult = await basketRefitService.AddBasketItemAsync(request);
        if (!responseAsResult.IsSuccessStatusCode)
        {
            logger.LogProblemDetailsExtension(responseAsResult.Error as ApiException);
            return ServiceResult.Error("An error occurred while creating or updating the basket");
        }
        
        return ServiceResult.Success();
    }
    
    public async Task<ServiceResult<BasketViewModel>> GetBasketsAsync()
    {
        var responseAsResult = await basketRefitService.GetBasketsAsync();
        if (!responseAsResult.IsSuccessStatusCode)
        {
            if (responseAsResult.StatusCode == HttpStatusCode.NotFound)
                return ServiceResult<BasketViewModel>.Success(BasketViewModel.Empty());
            
            logger.LogProblemDetailsExtension(responseAsResult.Error as ApiException);
            return ServiceResult<BasketViewModel>.Error("An error occurred while getting the baskets");
        }
        
        var basketViewModel = new BasketViewModel(
            responseAsResult.Content!.DiscountRate,
            responseAsResult.Content.CouponCode,
            responseAsResult.Content.TotalPrice,
            responseAsResult.Content.TotalDiscountedPrice,
            [
                .. responseAsResult.Content.Items.Select(item => new BasketItemViewModel(
                    item.CourseId,
                    item.CourseName,
                    item.CourseImageUrl,
                    item.CoursePrice,
                    item.DiscountedPrice
                ))
            ]
        );

        return ServiceResult<BasketViewModel>.Success(basketViewModel);
    }
    
    public async Task<ServiceResult<BasketPageViewModel>> GetBasketPageViewModelAsync()
    {
        var basketsAsResult = await GetBasketsAsync();
        if (basketsAsResult.IsFail)
            return ServiceResult<BasketPageViewModel>.Error(basketsAsResult.Fail!);

        var basketPageViewModel = new BasketPageViewModel();
        basketPageViewModel.SetPrice(basketsAsResult.Data!.TotalPrice, basketsAsResult.Data.TotalDiscountedPrice);
        basketPageViewModel.DiscountRate = basketsAsResult.Data.DiscountRate;
        basketPageViewModel.CouponCode = basketsAsResult.Data.CouponCode;
        
        foreach (var basketItem in basketsAsResult.Data!.Items)
            basketPageViewModel.Items.Add(new BasketViewModelItem(basketItem.CourseId, basketItem.CourseImageUrl,
                basketItem.CourseName, basketItem.CoursePrice, basketItem.DiscountedPrice));
        
        return ServiceResult<BasketPageViewModel>.Success(basketPageViewModel);
    }
    
    public async Task<ServiceResult> DeleteBasketAsync(Guid courseId)
    {
        var responseAsResult = await basketRefitService.DeleteItemAsync(courseId);
        if (!responseAsResult.IsSuccessStatusCode)
        {
            logger.LogProblemDetailsExtension(responseAsResult.Error as ApiException);
            return ServiceResult.Error("An error occurred while deleting the basket");
        }

        return ServiceResult.Success();
    }

    public async Task<ServiceResult> ApplyDiscountAsync(string coupon)
    {
        var discountResponseAsResult = await discountRefitService.GetDiscountByCouponCodeAsync(coupon);
        if (!discountResponseAsResult.IsSuccessStatusCode) return ServiceResult.FailFromProblemDetails((discountResponseAsResult.Error as ApiException)!);
        
        var discount = discountResponseAsResult.Content;
        var responseAsResult = await basketRefitService.ApplyDiscountRateAsync(new ApplyDiscountRateRequest(coupon, discount!.DiscountRate));
        if (!responseAsResult.IsSuccessStatusCode)
        {
            logger.LogProblemDetailsExtension(responseAsResult.Error as ApiException);
            return ServiceResult.Error("An error occurred while applying the discount");
        }
        
        return ServiceResult.Success();
    }
    
    public async Task<ServiceResult> RemoveDiscountAsync()
    {
        var response = await basketRefitService.RemoveDiscountRateAsync();
        if (!response.IsSuccessStatusCode)
        {
            logger.LogProblemDetailsExtension(response.Error as ApiException);
            return ServiceResult.Error("An error occurred while removing the discount");
        }

        return ServiceResult.Success();
    }
}