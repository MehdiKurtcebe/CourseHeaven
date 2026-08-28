namespace CourseHeaven.Web.Pages.Basket.ViewModel;

public record BasketViewModel(
    decimal? DiscountRate,
    string? CouponCode,
    decimal TotalPrice,
    decimal? TotalDiscountedPrice,
    List<BasketItemViewModel> Items)
{
    public static BasketViewModel Empty()
    {
        return new BasketViewModel(0, string.Empty, 0, 0, []);
    }
}