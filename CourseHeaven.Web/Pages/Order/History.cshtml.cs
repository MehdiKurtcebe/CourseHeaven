using CourseHeaven.Web.PageModels;
using CourseHeaven.Web.Pages.Order.ViewModel;
using CourseHeaven.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseHeaven.Web.Pages.Order;

[Authorize]
public class HistoryModel(OrderService orderService) : BasePageModel
{
    public List<OrderHistoryViewModel> OrderHistoryList { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync()
    {
        var response = await orderService.GetHistoryAsync();
        if (response.IsFail) return ErrorPage(response);

        OrderHistoryList = response.Data!;
        return Page();
    }
}