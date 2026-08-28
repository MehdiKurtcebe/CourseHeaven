using System.Text.Json.Serialization;

namespace CourseHeaven.Basket.Api.Data;

public class Basket
{
    public Basket(Guid userId, List<BasketItem> items)
    {
        UserId = userId;
        Items = items;
    }

    public Guid UserId { get; set; }

    public List<BasketItem> Items { get; set; } = [];

    public decimal? DiscountRate { get; set; }

    public string? CouponCode { get; set; }

    [JsonIgnore] public bool IsDiscountApplied => DiscountRate is > 0 && !string.IsNullOrEmpty(CouponCode);

    [JsonIgnore] public decimal TotalPrice => Items.Sum(item => item.CoursePrice);

    [JsonIgnore]
    public decimal? TotalDiscountedPrice => IsDiscountApplied ? Items.Sum(item => item.DiscountedPrice) : null;

    public void ApplyNewDiscount(string couponCode, decimal discountRate)
    {
        CouponCode = couponCode;
        DiscountRate = discountRate;

        foreach (var basketItem in Items)
            basketItem.DiscountedPrice = decimal.Ceiling(basketItem.CoursePrice * (1 - discountRate) * 100m) / 100m;
    }

    public void ApplyAvailableDiscount()
    {
        if (!IsDiscountApplied) return;

        foreach (var basketItem in Items)
            basketItem.DiscountedPrice =
                decimal.Ceiling((decimal)(basketItem.CoursePrice * (1 - DiscountRate) * 100m)!) / 100m;
    }

    public void ClearDiscount()
    {
        DiscountRate = null;
        CouponCode = null;
        foreach (var basketItem in Items) basketItem.DiscountedPrice = null;
    }
}