using CourseHeaven.Web.Pages.Basket.ViewModel;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CourseHeaven.Web.Pages.Order.ViewModel;

public record CreateOrderViewModel
{
    public string Address { get; set; } = null!;

    public PaymentViewModel Payment { get; set; } = null!;

    [ValidateNever] public List<OrderItemViewModel> OrderItems { get; set; } = [];


    [ValidateNever] public decimal? DiscountRate { get; set; }


    public decimal TotalPrice { get; set; }

    public static CreateOrderViewModel Empty => new()
    {
        Address = string.Empty,
        Payment = PaymentViewModel.Empty
    };


    public void AddOrderItem(BasketItemViewModel basketItem)
    {
        OrderItems.Add(new OrderItemViewModel(basketItem.CourseId, basketItem.CourseName,
            basketItem.DiscountedPrice ?? basketItem.CoursePrice));
    }
}