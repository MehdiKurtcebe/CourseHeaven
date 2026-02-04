namespace CourseHeaven.Basket.Api.Features.Baskets.Dtos;

public record BasketItemDto(
    Guid CourseId,
    string CourseName,
    decimal CoursePrice,
    decimal? DiscountedPrice,
    string? CourseImageUrl);