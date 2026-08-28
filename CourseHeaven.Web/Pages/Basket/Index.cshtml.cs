using CourseHeaven.Web.PageModels;
using CourseHeaven.Web.Pages.Basket.Dto;
using CourseHeaven.Web.Pages.Basket.ViewModel;
using CourseHeaven.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseHeaven.Web.Pages.Basket;

[Authorize]
public class IndexModel(CatalogService catalogService, BasketService basketService) : BasePageModel
{
    public BasketPageViewModel Basket { get; set; } = new();
    
    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
    {
        var basketAsResult = await basketService.GetBasketPageViewModelAsync();
        if (basketAsResult.IsFail)
            return ErrorPage(basketAsResult, "Index");

        Basket = basketAsResult.Data!;
        return Page();
    }
    
    public async Task<IActionResult> OnGetAddBasketAsync(Guid courseId, CancellationToken cancellationToken)
    {
        var course = await catalogService.GetCourseAsync(courseId);
        var createOrUpdateBasket = new AddBasketRequest(course.Data!.Id, course.Data.Name, course.Data.Price, course.Data.ImageUrl);

        var result = await basketService.CreateOrUpdateBasketAsync(createOrUpdateBasket);
        return result.IsFail ? ErrorPage(result, "Index") : SuccessPage("course added to basket", "Index");
    }

    public async Task<IActionResult> OnGetDeleteAsync(Guid courseId, CancellationToken cancellationToken)
    {
        var result = await basketService.DeleteBasketAsync(courseId);
        return result.IsFail ? ErrorPage(result, "Index") : SuccessPage("course deleted from basket", "Index");
    }

    public async Task<IActionResult> OnPostApplyDiscountAsync(string couponCode, CancellationToken cancellationToken)
    {
        var response = await basketService.ApplyDiscountAsync(couponCode);
        return response.IsFail ? ErrorPage(response, "Index") : SuccessPage("discount coupon applied", "Index");
    }

    public async Task<IActionResult> OnGetRemoveDiscountAsync(CancellationToken cancellationToken)
    {
        var response = await basketService.RemoveDiscountAsync();
        return response.IsFail ? ErrorPage(response, "Index") : SuccessPage("discount coupon removed", "Index");
    }
}