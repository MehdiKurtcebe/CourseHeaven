namespace CourseHeaven.Basket.Api.Dtos;

public record BasketItemDto(
    Guid CourseId,
    string CourseName,
    decimal CoursePrice,
    decimal? DiscountedPrice,
    string? CourseImageUrl);