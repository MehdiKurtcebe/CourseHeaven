namespace CourseHeaven.Web.Pages.Basket.Dto;

public record BasketResponse(
    decimal? DiscountRate,
    string? CouponCode,
    decimal TotalPrice,
    decimal? TotalDiscountedPrice,
    List<BasketItemDto> Items);