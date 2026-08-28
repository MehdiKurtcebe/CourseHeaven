using CourseHeaven.Web.Pages.Basket.Dto;
using Refit;

namespace CourseHeaven.Web.Services.Refit;

public interface IDiscountRefitService
{
    [Get("/api/v1/discounts/{couponCode}")]
    Task<ApiResponse<GetDiscountByCouponCodeResponse>> GetDiscountByCouponCodeAsync(string couponCode);
}