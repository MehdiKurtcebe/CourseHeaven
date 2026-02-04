namespace CourseHeaven.Basket.Api.Features.Baskets.Dtos;

public record BasketDto(Guid UserId, List<BasketItemDto> Items);