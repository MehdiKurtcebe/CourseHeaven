using System.Text.Json.Serialization;

namespace CourseHeaven.Basket.Api.Dtos;

public record BasketDto
{
    public required List<BasketItemDto> Items { get; init; }

    public decimal? DiscountRate { get; init; }

    public string? CouponCode { get; init; }

    [JsonIgnore] public bool IsDiscountApplied => DiscountRate is > 0 && !string.IsNullOrEmpty(CouponCode);

    public decimal TotalPrice => Items.Sum(item => item.CoursePrice);

    public decimal? TotalDiscountedPrice => IsDiscountApplied ? Items.Sum(item => item.DiscountedPrice) : null;
}