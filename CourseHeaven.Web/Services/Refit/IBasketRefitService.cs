using CourseHeaven.Web.Pages.Basket.Dto;
using Refit;

namespace CourseHeaven.Web.Services.Refit;

public interface IBasketRefitService
{
    [Post("/api/v1/baskets/items")]
    Task<ApiResponse<object>> AddBasketItemAsync(AddBasketRequest request);

    [Get("/api/v1/baskets/user")]
    Task<ApiResponse<BasketResponse>> GetBasketsAsync();
    
    [Put("/api/v1/baskets/discounts/apply")]
    Task<ApiResponse<object>> ApplyDiscountRateAsync(ApplyDiscountRateRequest applyDiscountRateRequest);
    
    [Delete("/api/v1/baskets/discounts/remove")]
    Task<ApiResponse<object>> RemoveDiscountRateAsync();
    
    [Delete("/api/v1/baskets/items/{courseId}")]
    Task<ApiResponse<object>> DeleteItemAsync(Guid courseId);
}