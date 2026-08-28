namespace CourseHeaven.Web.Pages.Basket.Dto;

public record BasketItemDto(
    Guid CourseId,
    string CourseName,
    decimal CoursePrice,
    string? CourseImageUrl,
    decimal? DiscountedPrice);