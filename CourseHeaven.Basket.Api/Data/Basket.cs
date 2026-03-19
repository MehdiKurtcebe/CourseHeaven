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

        foreach (var basketItem in Items) basketItem.DiscountedPrice = basketItem.CoursePrice * (1 - discountRate);
    }

    public void ApplyAvailableDiscount()
    {
        if (!IsDiscountApplied) return;

        foreach (var basketItem in Items) basketItem.DiscountedPrice = basketItem.CoursePrice * (1 - DiscountRate);
    }

    public void ClearDiscount()
    {
        DiscountRate = null;
        CouponCode = null;
        foreach (var basketItem in Items) basketItem.DiscountedPrice = null;
    }
}