namespace CourseHeaven.Web.Pages.Basket.ViewModel;

public record BasketPageViewModel
{
    public List<BasketViewModelItem> Items { get; set; } = [];

    private decimal TotalPrice { get; set; }

    private decimal? TotalDiscountedPrice { get; set; }
    public string? CouponCode { get; set; }
    public decimal? DiscountRate { get; set; }

    public bool IsDiscountApplied => DiscountRate is > 0 && !string.IsNullOrEmpty(CouponCode);

    public bool HasItem => Items.Count > 0;
    
    public decimal GetTotalPrice()
    {
        return IsDiscountApplied ? TotalDiscountedPrice!.Value : TotalPrice;
    }
    
    public void SetPrice(decimal totalPrice, decimal? totalPriceByDiscountRate)
    {
        TotalPrice = totalPrice;
        TotalDiscountedPrice = totalPriceByDiscountRate;
    }
}

public record BasketViewModelItem(
    Guid CourseId,
    string? CourseImageUrl,
    string CourseName,
    decimal CoursePrice,
    decimal? DiscountedPrice);